using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::MmsClientWinForms.Services;

   namespace MmsClientWinForms.Utils
   {
      public static class Validator
      {
      public static bool ValidateInputFields(TextBox txtLinePos, TextBox txtMac, out int lineNumber, out int lineLocation, out string mac)
      {
         lineNumber = 0;
         lineLocation = 0;
         mac = txtMac.Text.Trim();

         string input = txtLinePos.Text.Trim(); // 예: "Line: 1 | Position: 2"
         if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(mac))
         {
            MessageBox.Show("라인/위치번호 또는 MAC이 비어 있습니다.");
            return false;
         }

         var parts = input.Split('|');
         if (parts.Length != 2)
         {
            MessageBox.Show("입력 형식 오류 (예: Line: 1 | Position: 2)");
            return false;
         }

         string linePart = parts[0].Trim().Replace("Line:", "").Trim();
         string posPart = parts[1].Trim().Replace("Position:", "").Trim();

         if (!int.TryParse(linePart, out lineNumber) || !int.TryParse(posPart, out lineLocation))
         {
            MessageBox.Show("라인/위치번호 숫자 변환 실패 (예: Line: 1 | Position: 2)");
            return false;
         }

         if (!int.TryParse(SettingsService.Current.LineNo, out int expectedLineNo))
         {
            MessageBox.Show("설정 파일의 라인 번호가 잘못되어 있습니다.");
            return false;
         }

         if (lineNumber != expectedLineNo)
         {
            MessageBox.Show($"라인 번호 불일치! (입력: {lineNumber}, 설정: {expectedLineNo})");
            return false;
         }

         return true;
      }


      public static bool CheckDuplicateLocation(DataGridView dgvDevices, int location)
         {
            foreach (DataGridViewRow row in dgvDevices.Rows)
            {
               if (int.TryParse(row.Cells[0].Value?.ToString(), out int loc) && loc == location)
               {
                  MessageBox.Show($"위치 {location}은 이미 등록됨");
                  return true;
               }
            }
            return false;
         }

         public static bool CheckExpectedLocation(DataGridView dgvDevices, int location)
         {
            int max = 0;
            foreach (DataGridViewRow row in dgvDevices.Rows)
            {
               if (int.TryParse(row.Cells[0].Value?.ToString(), out int loc) && loc > max)
                  max = loc;
            }

            int expected = max + 1;
            if (location != expected)
            {
               MessageBox.Show($"잘못된 위치번호입니다. 다음 등록 위치는 {expected}번입니다.");
               return false;
            }

            return true;
         }

         public static bool CheckMacAlreadyExists(DataGridView dgvDevices, string mac)
         {
            foreach (DataGridViewRow row in dgvDevices.Rows)
            {
               if (row.Cells[1].Value?.ToString() == mac)
               {
                  MessageBox.Show("이미 등록된 MAC입니다.");
                  return true;
               }
            }
            return false;
         }
      }
   }


