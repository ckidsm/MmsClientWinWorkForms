using WsGatewayWorker.Services;
using System.Text.Json;

namespace WsGatewayWorker.Services
{
   public static class SettingsService
   {
      private static readonly string configPath = Path.Combine(AppContext.BaseDirectory, "config", "setting.json");

      public static Dictionary<string, object> Settings { get; private set; } = new();
      public static Settings Current { get; private set; } = new();

      public static void LoadFromJsonConfig()
      {
         try
         {
            if (!File.Exists(configPath))
            {
               FromJsonConfigDefaultFile(); // 메서드명 변경 반영
            }

            string json = File.ReadAllText(configPath);

            var loadedDict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            Settings = loadedDict ?? new Dictionary<string, object>();

            var loadedSettings = JsonSerializer.Deserialize<Settings>(json);
            Current = loadedSettings ?? new Settings();
         }
         catch (Exception ex)
         {
            Console.WriteLine($"[Settings] 설정 파일 로드 실패: {ex.Message}");
            FromJsonConfigDefaultFile(); // 예외 상황에서도 동일하게 사용
         }
      }

      public static void SaveFromJsonConfig(Dictionary<string, object> settings)
      {
         try
         {
            var dir = Path.GetDirectoryName(configPath);
            if (!string.IsNullOrEmpty(dir))
            {
               Directory.CreateDirectory(dir);
            }

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, json);

            Settings = settings;
            Current = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
         }
         catch (Exception ex)
         {
            Console.WriteLine($"[Settings] 저장 실패: {ex.Message}");
         }
      }

      /// <summary>
      /// 설정 파일이 존재하지 않을 경우 기본값으로 생성
      /// </summary>
      private static void FromJsonConfigDefaultFile()
      {
         var defaultSettings = new Dictionary<string, object>
         {
            ["master_server"] = "http://192.168.0.200:5000",
            ["local_server"] = "http://localhost:8080",
            ["line_number"] = 1,
            ["device_count_max"] = 30,
            ["device_type"] = "PCOS"
         };

         SaveFromJsonConfig(defaultSettings);
      }

      public static T Get<T>(string key, T defaultValue = default!)
      {
         if (Settings != null && Settings.TryGetValue(key, out object? value))
         {
            try
            {
               return (T)Convert.ChangeType(value, typeof(T));
            }
            catch { }
         }
         return defaultValue;
      }
   }
}