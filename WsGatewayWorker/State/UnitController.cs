using MmsClientWinForms.Enums;
using MmsClientWinForms.Models;
using MmsClientWinForms.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
namespace WsGatewayWorker.State
{
   /// <summary>
   /// UnitController는 단일 장비 그룹(Unit)의 상태, 시리얼 통신, 장비 등록 및 피딩 제어를 담당하는 클래스입니다.
   /// 장비 리스트(Device 목록)와 전체 상태를 관리하며, 메시지 큐를 통해 외부 메시지 수신도 처리합니다.
   /// </summary>
   public class UnitController
   {
      // 이 유닛의 고유 식별자
      public string UnitId { get; private set; }

      // 마스터 서버 주소 (예: http://localhost:5000)
      public string MasterServerUrl { get; private set; }

      // 유닛의 전체 상태 (예: EMPTY, RUNNING, COMPLETED 등)
      public UnitState State { get; private set; } = UnitState.EMPTY;

      // 등록된 장비(Device) 목록 (Key: DeviceId)
      private readonly Dictionary<string, Device> _devices = new();

      // 외부에서 장비 목록을 읽기 전용으로 접근 가능
      public IReadOnlyDictionary<string, Device> Devices => _devices;

      // 시리얼 통신용 서비스 객체
      private readonly SerialService? _serialService;

      // 외부 WebSocket 또는 내부 로직에서 전달되는 메시지를 비동기적으로 처리하기 위한 큐
      private readonly ConcurrentQueue<string> _messageQueue = new();

      // 외부에서 메시지 큐를 읽을 수 있도록 제공
      public ConcurrentQueue<string> MessageQueue => _messageQueue;

      // 마지막 피딩 실행 시간
      public DateTime? RunDate { get; private set; }

      // 몇 번째 피딩 실행인지 나타내는 카운터
      public int? NthRun { get; private set; }

      /// <summary>
      /// 생성자. 유닛 ID, 서버 주소, 시리얼 포트 설정을 받아 초기화합니다.
      /// 시리얼 서비스는 내부에서 자동으로 연결을 시도합니다.
      /// </summary>
      public UnitController(string unitId, string masterServerUrl, string serialPort = "COM1")
      {
         UnitId = unitId;
         MasterServerUrl = masterServerUrl;

         try
         {
            _serialService = new SerialService(serialPort, BroadcastSerialStatus);
            _serialService.Open(); // 시리얼 포트 열기
         }
         catch (Exception ex)
         {
            Console.WriteLine($"[ERROR] 시리얼 포트 오류: {ex.Message}");
         }
      }

      public async Task StartMessageHandlingLoop(CancellationToken token)
      {
         Console.WriteLine("[MSG_LOOP] 메시지 처리 루프 시작");

         while (!token.IsCancellationRequested)
         {
            if (TryDequeueMessage(out string? msg))
            {
               Console.WriteLine($"[MSG_LOOP] 수신: {msg}");

               switch (msg)
               {
                  case "commence":
                     await CommenceFeedingAsync();
                     break;
                  case "pause":
                     await PauseFeedingAsync();
                     break;
                  case "resume":
                     await ResumeFeedingAsync();
                     break;
                  case "conclude":
                     await ConcludeFeedingAsync();
                     break;
                  case "reset":
                     Reset();
                     break;
                  default:
                     Console.WriteLine($"[MSG_LOOP] 알 수 없는 명령: {msg}");
                     break;
               }
            }
            else
            {
               await Task.Delay(100); // 큐가 비었으면 잠시 대기
            }
         }

         Console.WriteLine("[MSG_LOOP] 메시지 루프 종료됨");
      }



      /// <summary>
      /// 외부 또는 내부에서 메시지를 큐에 추가
      /// </summary>
      public void EnqueueMessage(string msg) => _messageQueue.Enqueue(msg);

      /// <summary>
      /// 큐에서 메시지를 하나 꺼냄 (성공 시 true 반환)
      /// </summary>
      public bool TryDequeueMessage(out string? msg) => _messageQueue.TryDequeue(out msg);

      /// <summary>
      /// 유닛 상태 및 장비 목록 초기화
      /// </summary>
      public void Reset()
      {
         State = UnitState.EMPTY;
         _devices.Clear();
         RunDate = null;
         NthRun = null;
      }

      /// <summary>
      /// 새로운 장비를 유닛에 등록합니다. 중복된 ID는 무시됩니다.
      /// </summary>
      public void AddDevice(Device device)
      {
         if (!string.IsNullOrEmpty(device.DeviceId) && !_devices.ContainsKey(device.DeviceId))
            _devices[device.DeviceId] = device;
      }

      /// <summary>
      /// 특정 장비 ID에 해당하는 장비를 조회합니다.
      /// </summary>
      public Device? GetDevice(string deviceId)
      {
         return _devices.TryGetValue(deviceId, out var device) ? device : null;
      }

      /// <summary>
      /// 모든 장비가 IDLE 상태인지 확인합니다.
      /// </summary>
      public bool AllDevicesIdle() => _devices.Values.All(d => d.IsIdle);

      /// <summary>
      /// 모든 장비가 COMPLETED 또는 RETIRED 상태인지 확인합니다.
      /// </summary>
      public bool AllDevicesCompleted() => _devices.Values.All(d => d.IsCompleted || d.IsRetired);

      /// <summary>
      /// 시리얼 상태를 전파하는 내부 콜백 (UI 업데이트 또는 로깅 용도)
      /// </summary>
      private void BroadcastSerialStatus(string status)
      {
         // UI 또는 로그에 상태 전달 예정
      }

      /// <summary>
      /// 등록된 모든 장비에 피딩 시작 명령을 비동기로 보냅니다.
      /// 장비 상태가 IDLE인 경우만 CommenceFeeding 호출.
      /// </summary>
      public async Task CommenceFeedingAsync()
      {
         foreach (var device in _devices.Values)
         {
            if (device.IsIdle)
            {
               var service = new DeviceCommandService(device);
               await service.CommenceFeedingAsync();
            }
         }

         State = UnitState.RUNNING;
         RunDate = DateTime.Now;
         NthRun = (NthRun ?? 0) + 1;
      }

      /// <summary>
      /// 현재 피딩 중인 장비들에 Pause 명령 전송
      /// </summary>
      public async Task PauseFeedingAsync()
      {
         foreach (var device in _devices.Values)
         {
            if (device.IsFeeding)
            {
               var service = new DeviceCommandService(device);
               await service.PauseFeedingAsync();
            }
         }

         State = UnitState.PAUSED;
      }

      /// <summary>
      /// PAUSED 상태의 장비들에 Resume 명령 전송
      /// </summary>
      public async Task ResumeFeedingAsync()
      {
         foreach (var device in _devices.Values)
         {
            if (device.IsPaused)
            {
               var service = new DeviceCommandService(device);
               await service.ResumeFeedingAsync();
            }
         }

         State = UnitState.RUNNING;
      }

      /// <summary>
      /// 피딩을 마무리하고 상태를 COMPLETED로 전환합니다.
      /// </summary>
      public async Task ConcludeFeedingAsync()
      {
         foreach (var device in _devices.Values)
         {
            if (!device.IsStopped && !device.IsRetired)
            {
               var service = new DeviceCommandService(device);
               await service.ConcludeFeedingAsync();
            }
         }

         State = UnitState.COMPLETED;
      }

      public Task EnqueueMessageAsync(object obj)
      {
         string json = JsonSerializer.Serialize(obj);
         _messageQueue.Enqueue(json);
         return Task.CompletedTask;
      }

   }

}