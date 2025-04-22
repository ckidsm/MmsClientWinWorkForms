using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace MmsClientWinForms
{
   public partial class SettingsForm : Form
   {
      // 실행 파일 기준 상위 폴더의 config 폴더 내부 경로
      private static readonly string ConfigPath =
          Path.Combine(Directory.GetParent(Application.StartupPath)!.FullName, "config", "setting.json");

      public SettingsForm()
      {
         InitializeComponent();
         EnsureSettingsFile(); // 설정 파일 없으면 생성
         LoadSettings();       // 설정 파일 불러오기
      }

      /// <summary>
      /// 설정 파일이 없으면 기본 설정값으로 생성
      /// </summary>
      private void EnsureSettingsFile()
      {
         if (!File.Exists(ConfigPath))
         {
            var defaultSettings = new Dictionary<string, object>
            {
               ["master_server"] = "http://192.168.0.200:5000",
               ["local_server"] = "http://localhost:8080",
               ["line_number"] = 1,
               ["device_count_max"] = 30,
               ["device_type"] = "PCOS"
            };

            var dir = Path.GetDirectoryName(ConfigPath);
            if (!string.IsNullOrEmpty(dir))
            {
               Directory.CreateDirectory(dir);
            }

            var json = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
         }
      }

      /// <summary>
      /// 설정 파일을 읽어 폼에 반영
      /// </summary>
      private void LoadSettings()
      {
         try
         {
            var json = File.ReadAllText(ConfigPath);
            var settings = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            txtMasterServer.Text = settings?.GetValueOrDefault("master_server", "")?.ToString() ?? "";
            txtLocalServer.Text = settings?.GetValueOrDefault("local_server", "")?.ToString() ?? "";
            cmbLineNo.SelectedItem = settings?.GetValueOrDefault("line_number", "1")?.ToString() ?? "1";
            cmbDeviceCount.SelectedItem = settings?.GetValueOrDefault("device_count_max", "1")?.ToString() ?? "1";
            cmbDeviceType.SelectedItem = settings?.GetValueOrDefault("device_type", "PCOS")?.ToString() ?? "PCOS";
         }
         catch (Exception ex)
         {
            MessageBox.Show("설정 파일 로드 중 오류 발생: " + ex.Message);
         }
      }

      /// <summary>
      /// 현재 UI 입력값을 저장 파일로 기록
      /// </summary>
      private void SaveSettings()
      {
         string? lineNoStr = cmbLineNo.SelectedItem?.ToString();
         string? deviceCountStr = cmbDeviceCount.SelectedItem?.ToString();
         string? deviceTypeStr = cmbDeviceType.SelectedItem?.ToString();

         int lineNo = int.TryParse(lineNoStr, out var ln) ? ln : 1;
         int deviceCount = int.TryParse(deviceCountStr, out var dc) ? dc : 1;
         string deviceType = deviceTypeStr ?? "PCOS";

         var settings = new Dictionary<string, object>
         {
            ["master_server"] = txtMasterServer.Text.Trim(),
            ["local_server"] = txtLocalServer.Text.Trim(),
            ["line_number"] = lineNo,
            ["device_count_max"] = deviceCount,
            ["device_type"] = deviceType
         };

         try
         {
            var dir = Path.GetDirectoryName(ConfigPath);
            if (!string.IsNullOrEmpty(dir))
            {
               Directory.CreateDirectory(dir);
            }

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
            MessageBox.Show("설정이 저장되었습니다.");
            this.DialogResult = DialogResult.OK;
            this.Close();
         }
         catch (Exception ex)
         {
            MessageBox.Show("설정 저장 실패: " + ex.Message);
         }
      }

      private void btnSave_Click(object sender, EventArgs e)
      {
         SaveSettings();
      }
   }
}
