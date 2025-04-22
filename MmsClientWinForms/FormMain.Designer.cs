
using System.Drawing;
using System.Windows.Forms;

namespace MmsClientWinForms
{
   partial class FormMain
   {
      private Label lblServerStatus;
      private Label lblMacInput;
      private Label lblLinePosition;
      private Label lblLineInfo;
      private Label lblDeviceCount;
      private Label lblDeviceType;
      private Label lblStatusMessage;

      private TextBox txtMac;
      private TextBox txtLinePosition;
      private Button btnRegister;
      private DataGridView dgvDevices;

      private DataGridViewTextBoxColumn colLocation;
      private DataGridViewTextBoxColumn colMac;
      private DataGridViewTextBoxColumn colStatus;

      private void InitializeComponent()
      {
         DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
         lblServerStatus = new Label();
         lblLinePosition = new Label();
         txtLinePosition = new TextBox();
         lblMacInput = new Label();
         txtMac = new TextBox();
         btnRegister = new Button();
         dgvDevices = new DataGridView();
         colLocation = new DataGridViewTextBoxColumn();
         colMac = new DataGridViewTextBoxColumn();
         colStatus = new DataGridViewTextBoxColumn();
         lblLineInfo = new Label();
         lblDeviceCount = new Label();
         lblDeviceType = new Label();
         lblStatusMessage = new Label();
         btnSetting = new Button();
         ((System.ComponentModel.ISupportInitialize)dgvDevices).BeginInit();
         SuspendLayout();
         // 
         // lblServerStatus
         // 
         lblServerStatus.AutoSize = true;
         lblServerStatus.BackColor = Color.DimGray;
         lblServerStatus.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
         lblServerStatus.ForeColor = Color.White;
         lblServerStatus.Location = new Point(10, 10);
         lblServerStatus.Name = "lblServerStatus";
         lblServerStatus.Padding = new Padding(6);
         lblServerStatus.Size = new Size(243, 37);
         lblServerStatus.TabIndex = 0;
         lblServerStatus.Text = "서버 연결 상태: 연결 중...";
         lblServerStatus.TextAlign = ContentAlignment.TopCenter;
         // 
         // lblLinePosition
         // 
         lblLinePosition.AutoSize = true;
         lblLinePosition.ForeColor = Color.White;
         lblLinePosition.Location = new Point(12, 58);
         lblLinePosition.Name = "lblLinePosition";
         lblLinePosition.Size = new Size(214, 24);
         lblLinePosition.TabIndex = 1;
         lblLinePosition.Text = "라인 번호  및 위치번호";
         // 
         // txtLinePosition
         // 
         txtLinePosition.Location = new Point(232, 58);
         txtLinePosition.Name = "txtLinePosition";
         txtLinePosition.Size = new Size(272, 32);
         txtLinePosition.TabIndex = 2;
         txtLinePosition.KeyDown += TxtLinePosition_KeyDown;
         // 
         // lblMacInput
         // 
         lblMacInput.AutoSize = true;
         lblMacInput.ForeColor = Color.White;
         lblMacInput.Location = new Point(555, 58);
         lblMacInput.Name = "lblMacInput";
         lblMacInput.Size = new Size(105, 24);
         lblMacInput.TabIndex = 3;
         lblMacInput.Text = "장비 MAC";
         // 
         // txtMac
         // 
         txtMac.Location = new Point(679, 52);
         txtMac.Name = "txtMac";
         txtMac.Size = new Size(250, 32);
         txtMac.TabIndex = 4;
         txtMac.KeyDown += TxtMac_KeyDown;
         // 
         // btnRegister
         // 
         btnRegister.BackColor = Color.Gray;
         btnRegister.ForeColor = Color.White;
         btnRegister.Location = new Point(712, 10);
         btnRegister.Name = "btnRegister";
         btnRegister.Size = new Size(80, 32);
         btnRegister.TabIndex = 5;
         btnRegister.Text = "등록";
         btnRegister.UseVisualStyleBackColor = false;
         btnRegister.Click += btnRegister_Click;
         // 
         // dgvDevices
         // 
         dgvDevices.AllowUserToAddRows = false;
         dgvDevices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
         dgvDevices.BackgroundColor = Color.White;
         dataGridViewCellStyle1.Font = new Font("나눔고딕", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
         dgvDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
         dgvDevices.Columns.AddRange(new DataGridViewColumn[] { colLocation, colMac, colStatus });
         dgvDevices.EnableHeadersVisualStyles = false;
         dgvDevices.Location = new Point(10, 116);
         dgvDevices.Name = "dgvDevices";
         dgvDevices.ReadOnly = true;
         dgvDevices.RowHeadersVisible = false;
         dgvDevices.Size = new Size(960, 409);
         dgvDevices.TabIndex = 6;
         // 
         // colLocation
         // 
         colLocation.HeaderText = "Location";
         colLocation.Name = "colLocation";
         colLocation.ReadOnly = true;
         // 
         // colMac
         // 
         colMac.HeaderText = "Device Mac";
         colMac.Name = "colMac";
         colMac.ReadOnly = true;
         colMac.Width = 300;
         // 
         // colStatus
         // 
         colStatus.HeaderText = "Status";
         colStatus.Name = "colStatus";
         colStatus.ReadOnly = true;
         colStatus.Width = 300;
         // 
         // lblLineInfo
         // 
         lblLineInfo.AutoSize = true;
         lblLineInfo.ForeColor = Color.White;
         lblLineInfo.Location = new Point(10, 560);
         lblLineInfo.Name = "lblLineInfo";
         lblLineInfo.Size = new Size(121, 24);
         lblLineInfo.TabIndex = 7;
         lblLineInfo.Text = "라인 번호: 1";
         // 
         // lblDeviceCount
         // 
         lblDeviceCount.AutoSize = true;
         lblDeviceCount.ForeColor = Color.White;
         lblDeviceCount.Location = new Point(430, 560);
         lblDeviceCount.Name = "lblDeviceCount";
         lblDeviceCount.Size = new Size(147, 24);
         lblDeviceCount.TabIndex = 8;
         lblDeviceCount.Text = "검사 장비 수: 0";
         // 
         // lblDeviceType
         // 
         lblDeviceType.AutoSize = true;
         lblDeviceType.ForeColor = Color.White;
         lblDeviceType.Location = new Point(760, 560);
         lblDeviceType.Name = "lblDeviceType";
         lblDeviceType.Size = new Size(208, 24);
         lblDeviceType.TabIndex = 9;
         lblDeviceType.Text = "검사 장비 타입: PCOS";
         // 
         // lblStatusMessage
         // 
         lblStatusMessage.BackColor = Color.White;
         lblStatusMessage.Dock = DockStyle.Bottom;
         lblStatusMessage.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
         lblStatusMessage.ForeColor = Color.White;
         lblStatusMessage.Location = new Point(0, 599);
         lblStatusMessage.Name = "lblStatusMessage";
         lblStatusMessage.Size = new Size(1000, 36);
         lblStatusMessage.TabIndex = 10;
         lblStatusMessage.TextAlign = ContentAlignment.MiddleCenter;
         // 
         // btnSetting
         // 
         btnSetting.BackColor = Color.Gray;
         btnSetting.ForeColor = Color.White;
         btnSetting.Location = new Point(890, 10);
         btnSetting.Name = "btnSetting";
         btnSetting.Size = new Size(80, 32);
         btnSetting.TabIndex = 5;
         btnSetting.Text = "설정";
         btnSetting.UseVisualStyleBackColor = false;
         // 
         // FormMain
         // 
         AutoScaleMode = AutoScaleMode.None;
         BackColor = Color.FromArgb(64, 64, 64);
         ClientSize = new Size(1000, 635);
         Controls.Add(lblServerStatus);
         Controls.Add(lblLinePosition);
         Controls.Add(txtLinePosition);
         Controls.Add(lblMacInput);
         Controls.Add(txtMac);
         Controls.Add(btnSetting);
         Controls.Add(btnRegister);
         Controls.Add(dgvDevices);
         Controls.Add(lblLineInfo);
         Controls.Add(lblDeviceCount);
         Controls.Add(lblDeviceType);
         Controls.Add(lblStatusMessage);
         Font = new Font("나눔고딕", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
         MinimumSize = new Size(600, 400);
         Name = "FormMain";
         Text = "장비 등록 상태 화면 (.NET 8)";
         FormClosing += FormMain_FormClosing;
         Load += FormMain_Load;
         ((System.ComponentModel.ISupportInitialize)dgvDevices).EndInit();
         ResumeLayout(false);
         PerformLayout();
      }
      private Button btnSetting;
   }
}
