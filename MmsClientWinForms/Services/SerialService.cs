using MmsClientWinForms.Models;
using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace MmsClientWinForms.Services
{
   public enum FeederStatus
   {
      IDLE,
      RUNNING,
      STOPPED,
      JAM,
      TIMEOUT,
      UNKNOWN
   }

   public class SerialService
   {
      private readonly string _portName;
      private SerialPort? _serialPort; // nullable 허용
      private readonly Action<string>? _statusCallback;

      public FeederStatus Status { get; private set; } = FeederStatus.IDLE;

      private static readonly byte[] FEEDER_CMD_START = { 0x2B, 0x01, 0x2A, 0x0D };
      private static readonly byte[] FEEDER_CMD_STOP = { 0x2B, 0x02, 0x29, 0x0D };
      private static readonly byte[] FEEDER_RET_SUC = { 0x06, 0x0D };
      private static readonly byte[] FEEDER_RET_FAIL = { 0x15, 0x0D };
      private static readonly byte[] FEEDER_RET_JAM = { 0x10, 0x0D };

      public SerialService(string portName, Action<string>? statusCallback = null)
      {
         _portName = portName;
         _statusCallback = statusCallback;
      }

      public void Open()
      {
         _serialPort = new SerialPort(_portName, 115200, Parity.None, 8, StopBits.One);
         _serialPort.ReadTimeout = 1000;
         _serialPort.Open();
      }

      public async Task StartAsync()
      {
         if (_serialPort?.IsOpen != true) return;

         Status = FeederStatus.RUNNING;
         _serialPort.Write(FEEDER_CMD_START, 0, FEEDER_CMD_START.Length);

         await Task.Delay(200); // 대기 후 결과 확인

         byte[] buffer = new byte[2];
         try
         {
            _serialPort.Read(buffer, 0, 2);
            if (Match(buffer, FEEDER_RET_SUC))
               Status = FeederStatus.RUNNING;
            else if (Match(buffer, FEEDER_RET_JAM))
               Status = FeederStatus.JAM;
            else
               Status = FeederStatus.UNKNOWN;
         }
         catch
         {
            Status = FeederStatus.TIMEOUT;
         }

         _statusCallback?.Invoke(Status.ToString());
      }

      public void Stop()
      {
         if (_serialPort?.IsOpen != true) return;

         _serialPort.Write(FEEDER_CMD_STOP, 0, FEEDER_CMD_STOP.Length);
         Status = FeederStatus.STOPPED;
         _statusCallback?.Invoke(Status.ToString());
      }

      private bool Match(byte[] input, byte[] expected)
      {
         if (input.Length != expected.Length) return false;
         for (int i = 0; i < input.Length; i++)
            if (input[i] != expected[i]) return false;
         return true;
      }

      public void Send(string message)
      {
         if (_serialPort?.IsOpen == true)
         {
            _serialPort.Write(message);
         }
      }
   }
}


