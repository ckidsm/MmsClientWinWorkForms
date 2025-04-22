namespace MmsClientWinForms
{
   partial class SettingsForm
   {
      private System.ComponentModel.IContainer components = null;

      private System.Windows.Forms.TextBox txtMasterServer;
      private System.Windows.Forms.TextBox txtLocalServer;
      private System.Windows.Forms.ComboBox cmbLineNo;
      private System.Windows.Forms.ComboBox cmbDeviceCount;
      private System.Windows.Forms.ComboBox cmbDeviceType;
      private System.Windows.Forms.Button btnSave;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
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
         txtMasterServer = new TextBox();
         txtLocalServer = new TextBox();
         cmbLineNo = new ComboBox();
         cmbDeviceCount = new ComboBox();
         cmbDeviceType = new ComboBox();
         btnSave = new Button();
         lbl1 = new Label();
         lbl2 = new Label();
         lbl3 = new Label();
         lbl4 = new Label();
         lbl5 = new Label();
         SuspendLayout();
         // 
         // txtMasterServer
         // 
         txtMasterServer.Location = new Point(20, 56);
         txtMasterServer.Name = "txtMasterServer";
         txtMasterServer.Size = new Size(391, 39);
         txtMasterServer.TabIndex = 1;
         // 
         // txtLocalServer
         // 
         txtLocalServer.Location = new Point(20, 143);
         txtLocalServer.Name = "txtLocalServer";
         txtLocalServer.Size = new Size(391, 39);
         txtLocalServer.TabIndex = 3;
         // 
         // cmbLineNo
         // 
         cmbLineNo.DropDownStyle = ComboBoxStyle.DropDownList;
         cmbLineNo.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
         cmbLineNo.Location = new Point(20, 229);
         cmbLineNo.Name = "cmbLineNo";
         cmbLineNo.Size = new Size(100, 40);
         cmbLineNo.TabIndex = 5;
         // 
         // cmbDeviceCount
         // 
         cmbDeviceCount.DropDownStyle = ComboBoxStyle.DropDownList;
         cmbDeviceCount.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" });
         cmbDeviceCount.Location = new Point(156, 229);
         cmbDeviceCount.Name = "cmbDeviceCount";
         cmbDeviceCount.Size = new Size(100, 40);
         cmbDeviceCount.TabIndex = 7;
         // 
         // cmbDeviceType
         // 
         cmbDeviceType.DropDownStyle = ComboBoxStyle.DropDownList;
         cmbDeviceType.Items.AddRange(new object[] { "PCOS", "VVD" });
         cmbDeviceType.Location = new Point(293, 229);
         cmbDeviceType.Name = "cmbDeviceType";
         cmbDeviceType.Size = new Size(120, 40);
         cmbDeviceType.TabIndex = 9;
         // 
         // btnSave
         // 
         btnSave.Location = new Point(311, 307);
         btnSave.Name = "btnSave";
         btnSave.Size = new Size(121, 42);
         btnSave.TabIndex = 10;
         btnSave.Text = "저장";
         btnSave.Click += btnSave_Click;
         // 
         // lbl1
         // 
         lbl1.AutoSize = true;
         lbl1.Location = new Point(20, 20);
         lbl1.Name = "lbl1";
         lbl1.Size = new Size(166, 32);
         lbl1.TabIndex = 0;
         lbl1.Text = "메인서버 주소";
         // 
         // lbl2
         // 
         lbl2.AutoSize = true;
         lbl2.Location = new Point(20, 107);
         lbl2.Name = "lbl2";
         lbl2.Size = new Size(174, 32);
         lbl2.TabIndex = 2;
         lbl2.Text = "로컬 서버 주소";
         // 
         // lbl3
         // 
         lbl3.AutoSize = true;
         lbl3.Location = new Point(20, 194);
         lbl3.Name = "lbl3";
         lbl3.Size = new Size(118, 32);
         lbl3.TabIndex = 4;
         lbl3.Text = "라인 번호";
         // 
         // lbl4
         // 
         lbl4.AutoSize = true;
         lbl4.Location = new Point(156, 194);
         lbl4.Name = "lbl4";
         lbl4.Size = new Size(94, 32);
         lbl4.TabIndex = 6;
         lbl4.Text = "장비 수";
         // 
         // lbl5
         // 
         lbl5.AutoSize = true;
         lbl5.Location = new Point(293, 194);
         lbl5.Name = "lbl5";
         lbl5.Size = new Size(118, 32);
         lbl5.TabIndex = 8;
         lbl5.Text = "장비 유형";
         // 
         // SettingsForm
         // 
         ClientSize = new Size(444, 371);
         Controls.Add(lbl1);
         Controls.Add(txtMasterServer);
         Controls.Add(lbl2);
         Controls.Add(txtLocalServer);
         Controls.Add(lbl3);
         Controls.Add(cmbLineNo);
         Controls.Add(lbl4);
         Controls.Add(cmbDeviceCount);
         Controls.Add(lbl5);
         Controls.Add(cmbDeviceType);
         Controls.Add(btnSave);
         Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point, 129);
         FormBorderStyle = FormBorderStyle.FixedDialog;
         MaximizeBox = false;
         Name = "SettingsForm";
         StartPosition = FormStartPosition.CenterParent;
         Text = "설정";
         TopMost = true;
         ResumeLayout(false);
         PerformLayout();
      }

      #endregion

      private Label lbl1;
      private Label lbl2;
      private Label lbl3;
      private Label lbl4;
      private Label lbl5;
   }
}
