using MmsClientWinForms.Models;
using MmsClientWinForms.Services;

namespace MmsClientWinForms
{
   public partial class Form1 : Form
   {
      private readonly ProductSerialService _serialService;

      public Form1(ProductSerialService serialService)
      {
         InitializeComponent();
         _serialService = serialService;
      }

      private async void btnSend_Click(object sender, EventArgs e)
      {
         var request = new ProductSerialRequest
         {
            Type = txtType.Text.Trim(),
            Local = string.IsNullOrWhiteSpace(txtLocal.Text) ? null : txtLocal.Text,
            BoardSerial = txtBoardSerial.Text.Trim(),
            MacAddress = txtMacAddress.Text.Trim()
         };

         var response = await _serialService.IssueSerialAsync(request);

         if (response == null)
         {
            MessageBox.Show("서버 응답 없음", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         txtResponse.Text = $"[{response.Status}] {response.Message}\n"
                          + (response.Data != null
                              ? $"{response.Data.ProductSerial} / {response.Data.QrContent}"
                              : "No Data");
      }
   }
}
