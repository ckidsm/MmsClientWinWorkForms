namespace MmsClientWinForms
{
   partial class Form1
   {
      private System.ComponentModel.IContainer components = null;

      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      private void InitializeComponent()
      {
         this.txtType = new System.Windows.Forms.TextBox();
         this.txtLocal = new System.Windows.Forms.TextBox();
         this.txtBoardSerial = new System.Windows.Forms.TextBox();
         this.txtMacAddress = new System.Windows.Forms.TextBox();
         this.btnSend = new System.Windows.Forms.Button();
         this.txtResponse = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // txtType
         // 
         this.txtType.Location = new System.Drawing.Point(12, 12);
         this.txtType.Name = "txtType";
         this.txtType.PlaceholderText = "장비 타입 (예: PCOS, VVD)";
         this.txtType.Size = new System.Drawing.Size(300, 23);
         this.txtType.TabIndex = 0;
         // 
         // txtLocal
         // 
         this.txtLocal.Location = new System.Drawing.Point(12, 41);
         this.txtLocal.Name = "txtLocal";
         this.txtLocal.PlaceholderText = "지역 (예: WES, HUGO) / 비우면 null";
         this.txtLocal.Size = new System.Drawing.Size(300, 23);
         this.txtLocal.TabIndex = 1;
         // 
         // txtBoardSerial
         // 
         this.txtBoardSerial.Location = new System.Drawing.Point(12, 70);
         this.txtBoardSerial.Name = "txtBoardSerial";
         this.txtBoardSerial.PlaceholderText = "보드 시리얼 (예: IRQPC254000001)";
         this.txtBoardSerial.Size = new System.Drawing.Size(300, 23);
         this.txtBoardSerial.TabIndex = 2;
         // 
         // txtMacAddress
         // 
         this.txtMacAddress.Location = new System.Drawing.Point(12, 99);
         this.txtMacAddress.Name = "txtMacAddress";
         this.txtMacAddress.PlaceholderText = "MAC 주소 (예: 10-FC-B6-10-00-01)";
         this.txtMacAddress.Size = new System.Drawing.Size(300, 23);
         this.txtMacAddress.TabIndex = 3;
         // 
         // btnSend
         // 
         this.btnSend.Location = new System.Drawing.Point(12, 128);
         this.btnSend.Name = "btnSend";
         this.btnSend.Size = new System.Drawing.Size(300, 30);
         this.btnSend.TabIndex = 4;
         this.btnSend.Text = "P/S 발급 요청";
         this.btnSend.UseVisualStyleBackColor = true;
         this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
         // 
         // txtResponse
         // 
         this.txtResponse.Location = new System.Drawing.Point(12, 164);
         this.txtResponse.Multiline = true;
         this.txtResponse.Name = "txtResponse";
         this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.txtResponse.Size = new System.Drawing.Size(500, 200);
         this.txtResponse.TabIndex = 5;
         // 
         // Form1
         // 
         this.ClientSize = new System.Drawing.Size(524, 380);
         this.Controls.Add(this.txtResponse);
         this.Controls.Add(this.btnSend);
         this.Controls.Add(this.txtMacAddress);
         this.Controls.Add(this.txtBoardSerial);
         this.Controls.Add(this.txtLocal);
         this.Controls.Add(this.txtType);
         this.Name = "Form1";
         this.Text = "Product Serial 발급 테스트";
         this.ResumeLayout(false);
         this.PerformLayout();
      }

      #endregion

      private System.Windows.Forms.TextBox txtType;
      private System.Windows.Forms.TextBox txtLocal;
      private System.Windows.Forms.TextBox txtBoardSerial;
      private System.Windows.Forms.TextBox txtMacAddress;
      private System.Windows.Forms.Button btnSend;
      private System.Windows.Forms.TextBox txtResponse;
   }
}