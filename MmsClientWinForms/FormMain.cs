using System.Text.Json;
using MmsClientWinForms.Services;
using MmsClientWinForms.State;
using MmsClientWinForms.Enums;
using MmsClientWinForms.Utils;
using System.Net.WebSockets;
using MmsClientWinForms.Utils.MmsClientWinForms.Utils;

namespace MmsClientWinForms
{
   public partial class FormMain : Form
   {
      private readonly ApiService _apiService = new();
      private readonly WebSocketClient _wsClient;
      private readonly SerialService _serialService;
      private readonly UnitController _unitController;
      private CancellationTokenSource? _msgLoopCts;
      private ClientWebSocket _client = new ClientWebSocket();


      public FormMain()
      {
         InitializeComponent();
         SettingsService.LoadFromJsonConfig();


         string wsUri = SettingsService.Get("external_ws_uri", "ws://localhost:5000/ws");
         string masterUrl = SettingsService.Get("master_server", "http://192.168.0.200:5000");

         _unitController = new UnitController("UNIT001", masterUrl, SettingsService.Get("serial_port", "COM1"));
         _serialService = new SerialService(SettingsService.Get("serial_port", "COM1"), UpdateSerialStatus);
         _wsClient = new WebSocketClient(wsUri);
         _wsClient.OnMessageReceived += HandleWebSocketMessage;
        // _wsClient.OnStatusChanged += UpdateWebSocketStatus; // ✅ 상태 변경 시 UI 업데이트
         _wsClient.Connect();

         lblServerStatus.Text = "서버 연결 상태: 연결 중...";
      }

      private void FormMain_Load(object sender, EventArgs e)
      {
         UpdateSettingLabels();
        // UpdateWebSocketStatus();
         StartBackgroundMessageLoop();
      }

      private async void TxtMac_KeyDown(object? sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
         {
            e.SuppressKeyPress = true;
            if (!Validator.ValidateInputFields(txtLinePosition, txtMac, out int lineNumber, out int lineLocation, out string mac)) return;
            if (Validator.CheckDuplicateLocation(dgvDevices, lineLocation)) return;
            if (!Validator.CheckExpectedLocation(dgvDevices, lineLocation)) return;
            if (Validator.CheckMacAlreadyExists(dgvDevices, mac)) return;
            await RegisterToServer();
         }
      }

      private async void TxtLinePosition_KeyDown(object? sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
         {
            e.SuppressKeyPress = true;
            if (!Validator.ValidateInputFields(txtLinePosition, txtMac, out int lineNumber, out int lineLocation, out string mac)) return;
            if (Validator.CheckDuplicateLocation(dgvDevices, lineLocation)) return;
            if (!Validator.CheckExpectedLocation(dgvDevices, lineLocation)) return;
            if (Validator.CheckMacAlreadyExists(dgvDevices, mac)) return;
            await RegisterToServer();
         }
      }

      private async Task RegisterToServer()
      {
         string baseUrl = SettingsService.Get("master_server", "http://localhost:5000");
         string getUrl = $"{baseUrl}/api/QualityCheckNumber/GetLineCheckNumbers";
         string postUrl = $"{baseUrl}/api/QualityCheckNumber/AddLineCheckNumber";

         string mac = txtMac.Text.Trim();
         string lineInput = txtLinePosition.Text.Trim();
         int lineNumber, lineLocation;

         try
         {
            var parts = lineInput.Split('|');
            lineNumber = int.Parse(parts[0].Trim().Replace("Line:", ""));
            lineLocation = int.Parse(parts[1].Trim().Replace("Position:", ""));
         }
         catch
         {
            ShowErrorPopup("입력 형식 오류 (예: Line: 1 | Position: 2)");
            return;
         }

         var getParams = new Dictionary<string, string>
         {
            { "line_Number", lineNumber.ToString() },
            { "line_Location", lineLocation.ToString() },
            { "device_Mac_Address", mac }
         };

         string fullGetUrl = UrlHelper.AddQueryString(getUrl, getParams);

         using var client = new HttpClient();
         var getResponse = await client.GetAsync(fullGetUrl);
         if (getResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
         {
            ShowErrorPopup("이미 서버에 등록된 장비입니다.");
            return;
         }
         if (!getResponse.IsSuccessStatusCode)
         {
            ShowErrorPopup("중복 확인 중 오류가 발생했습니다.");
            return;
         }

         var postBody = new
         {
            Line_Number = lineNumber,
            Line_Location = lineLocation,
            Device_Mac_Address = mac,
            status = "Registered"
         };

         var content = new StringContent(JsonSerializer.Serialize(postBody), System.Text.Encoding.UTF8, "application/json");
         var postResponse = await client.PostAsync(postUrl, content);

         if (!postResponse.IsSuccessStatusCode)
         {
            ShowErrorPopup($"서버 오류: {postResponse.StatusCode}");
            return;
         }

         var json = JsonDocument.Parse(await postResponse.Content.ReadAsStringAsync());
         if (json.RootElement.GetProperty("success").GetBoolean())
         {
            dgvDevices.Rows.Add(lineLocation, mac, "등록됨");
            lblDeviceCount.Text = $"장비 수: {dgvDevices.Rows.Count}";
            ResetInputFields();
         }
         else
         {
            ShowErrorPopup("등록 실패: 서버 응답 오류");
         }
      }

      private void UpdateSettingLabels()
      {
         int lineNo = SettingsService.Get("line_number", 1);
         int deviceCount = SettingsService.Get("device_count_max", 1);
         string deviceType = SettingsService.Get("device_type", "PCOS");

         lblLineInfo.Text = $"라인 번호: {lineNo}";
         lblDeviceCount.Text = $"장비 수: {deviceCount}";
         lblDeviceType.Text = $"장비 유형: {deviceType}";
      }

      private void UpdateSerialStatus(string status)
      {
         Invoke(() => lblServerStatus.Text = $"시리얼 상태: {status}");
      }

      private void UpdateWebSocketStatus(string status)
      {
         Color bg = status switch
         {
            "connected" => Color.LightGreen,
            "timeout" => Color.LightCoral,
            "disconnected" => Color.Khaki,
            _ => Color.LightGray
         };
         SetStatusLabel($"서버 연결 상태: {status}", bg, Color.Black);
         SetFooterMessage($"WebSocket 상태: {status}", Color.DimGray, Color.White);
      }

      private void SetStatusLabel(string message, Color bgColor, Color textColor)
      {
         lblServerStatus.Text = message;
         lblServerStatus.BackColor = bgColor;
         lblServerStatus.ForeColor = textColor;
      }

      private void SetFooterMessage(string message, Color bgColor, Color textColor)
      {
         lblStatusMessage.Text = message;
         lblStatusMessage.BackColor = bgColor;
         lblStatusMessage.ForeColor = textColor;
         Console.WriteLine($"[STATUS] {message}");
      }

      private void ShowErrorPopup(string message)
      {
         MessageBox.Show(message, "오류 발생", MessageBoxButtons.OK, MessageBoxIcon.Error);
         Console.WriteLine($"[GUI] 상태표시: {message}");
      }

      private void ResetInputFields()
      {
         txtMac.Clear();
         txtLinePosition.Clear();
         txtLinePosition.Focus();
      }

      private void OpenSettingsDialog()
      {
         using var dlg = new SettingsForm();
         if (dlg.ShowDialog() == DialogResult.OK)
         {
            Console.WriteLine("설정이 저장되었습니다.");
            UpdateSettingLabels();
         }
      }

      private void StartBackgroundMessageLoop()
      {
         _msgLoopCts = new CancellationTokenSource();
         Task.Run(() => _unitController.StartMessageHandlingLoop(_msgLoopCts.Token));
      }

      private void StopBackgroundMessageLoop()
      {
         _msgLoopCts?.Cancel();
      }

      private void HandleWebSocketMessage(JsonElement json)
      {
         if (json.TryGetProperty("command", out var cmdElement))
         {
            string? cmd = cmdElement.GetString();
            if (!string.IsNullOrEmpty(cmd))
               _unitController.EnqueueMessage(cmd);
         }

         if (json.TryGetProperty("device_id", out var idElement))
         {
            string? deviceId = idElement.GetString();
            if (!string.IsNullOrEmpty(deviceId))
            {
               var device = _unitController.GetDevice(deviceId);
               if (device != null && json.TryGetProperty("state", out var state))
               {
                  Enum.TryParse(state.GetString(), out DeviceState parsedState);
                  device.State = parsedState;
                  Invoke(UpdateDeviceGrid);
               }
            }
         }
      }

      private void UpdateDeviceGrid()
      {
         dgvDevices.Rows.Clear();
         foreach (var d in _unitController.Devices.Values)
         {
            int row = dgvDevices.Rows.Add(d.DeviceId, d.BoardNo, d.State.ToString());
            dgvDevices.Rows[row].Cells[2].Style.BackColor =
               d.State == DeviceState.DS_COMPLETED ? Color.LightGreen : Color.Yellow;
         }
         lblDeviceCount.Text = $"장비 수: {_unitController.Devices.Count}";
      }

      private async void btnRegister_Click(object sender, EventArgs e)
      {
         await RegisterToServer();
      }

      private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
      {
         StopBackgroundMessageLoop();

      }
   }
}
