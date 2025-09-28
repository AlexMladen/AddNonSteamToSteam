namespace AddNonSteamToSteam
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtDisplayName = new TextBox();
            label2 = new Label();
            txtSteamName = new TextBox();
            label3 = new Label();
            txtExe = new TextBox();
            label4 = new Label();
            txtUrl = new TextBox();
            btnPickShortcut = new Button();
            label5 = new Label();
            txtSteamAppId = new TextBox();
            btnAddUpdate = new Button();
            titleBar = new Panel();
            btnMin = new Button();
            btnClose = new Button();
            rbUri = new RadioButton();
            rbExe = new RadioButton();
            titleBar.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(31, 155);
            label1.Name = "label1";
            label1.Size = new Size(80, 15);
            label1.TabIndex = 1;
            label1.Text = "Display Name";
            // 
            // txtDisplayName
            // 
            txtDisplayName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDisplayName.Location = new Point(31, 173);
            txtDisplayName.Name = "txtDisplayName";
            txtDisplayName.Size = new Size(736, 23);
            txtDisplayName.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(31, 350);
            label2.Name = "label2";
            label2.Size = new Size(113, 15);
            label2.TabIndex = 3;
            label2.Text = "Steam Search Name";
            // 
            // txtSteamName
            // 
            txtSteamName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSteamName.Location = new Point(31, 368);
            txtSteamName.Name = "txtSteamName";
            txtSteamName.Size = new Size(736, 23);
            txtSteamName.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(31, 278);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 6;
            label3.Text = "EXE Path";
            // 
            // txtExe
            // 
            txtExe.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtExe.Location = new Point(31, 301);
            txtExe.Name = "txtExe";
            txtExe.Size = new Size(736, 23);
            txtExe.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Location = new Point(31, 220);
            label4.Name = "label4";
            label4.Size = new Size(28, 15);
            label4.TabIndex = 8;
            label4.Text = "URL";
            // 
            // txtUrl
            // 
            txtUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUrl.Location = new Point(31, 238);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(736, 23);
            txtUrl.TabIndex = 9;
            // 
            // btnPickShortcut
            // 
            btnPickShortcut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPickShortcut.FlatStyle = FlatStyle.Flat;
            btnPickShortcut.Font = new Font("Verdana", 8.25F, FontStyle.Bold);
            btnPickShortcut.Location = new Point(468, 56);
            btnPickShortcut.Name = "btnPickShortcut";
            btnPickShortcut.Size = new Size(171, 80);
            btnPickShortcut.TabIndex = 10;
            btnPickShortcut.Text = "Pick Shortcut";
            btnPickShortcut.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Location = new Point(31, 412);
            label5.Name = "label5";
            label5.Size = new Size(75, 15);
            label5.TabIndex = 11;
            label5.Text = "Steam AppId";
            // 
            // txtSteamAppId
            // 
            txtSteamAppId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSteamAppId.Location = new Point(31, 430);
            txtSteamAppId.Name = "txtSteamAppId";
            txtSteamAppId.Size = new Size(736, 23);
            txtSteamAppId.TabIndex = 12;
            // 
            // btnAddUpdate
            // 
            btnAddUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddUpdate.FlatStyle = FlatStyle.Flat;
            btnAddUpdate.Font = new Font("Verdana", 8.25F, FontStyle.Bold);
            btnAddUpdate.Location = new Point(645, 56);
            btnAddUpdate.Name = "btnAddUpdate";
            btnAddUpdate.Size = new Size(122, 80);
            btnAddUpdate.TabIndex = 13;
            btnAddUpdate.Text = "Add \\ Update to Steam";
            btnAddUpdate.UseVisualStyleBackColor = true;
            // 
            // titleBar
            // 
            titleBar.BackColor = Color.FromArgb(255, 128, 255);
            titleBar.Controls.Add(btnMin);
            titleBar.Controls.Add(btnClose);
            titleBar.Dock = DockStyle.Top;
            titleBar.Location = new Point(0, 0);
            titleBar.Name = "titleBar";
            titleBar.Size = new Size(800, 36);
            titleBar.TabIndex = 14;
            titleBar.Paint += titleBar_Paint;
            // 
            // btnMin
            // 
            btnMin.FlatStyle = FlatStyle.Flat;
            btnMin.Location = new Point(731, 0);
            btnMin.Name = "btnMin";
            btnMin.Size = new Size(36, 36);
            btnMin.TabIndex = 1;
            btnMin.Text = "-";
            btnMin.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Location = new Point(764, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(36, 36);
            btnClose.TabIndex = 0;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // rbUri
            // 
            rbUri.Appearance = Appearance.Button;
            rbUri.Checked = true;
            rbUri.Location = new Point(63, 76);
            rbUri.Name = "rbUri";
            rbUri.Padding = new Padding(5, 0, 0, 0);
            rbUri.Size = new Size(160, 40);
            rbUri.TabIndex = 0;
            rbUri.TabStop = true;
            rbUri.Text = "Launch via Epic (or other)";
            rbUri.UseVisualStyleBackColor = true;
            // 
            // rbExe
            // 
            rbExe.Appearance = Appearance.Button;
            rbExe.Location = new Point(245, 76);
            rbExe.Name = "rbExe";
            rbExe.Padding = new Padding(10, 0, 0, 0);
            rbExe.Size = new Size(120, 40);
            rbExe.TabIndex = 1;
            rbExe.Text = "Launch via EXE";
            rbExe.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AcceptButton = btnAddUpdate;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 479);
            Controls.Add(rbUri);
            Controls.Add(rbExe);
            Controls.Add(titleBar);
            Controls.Add(btnAddUpdate);
            Controls.Add(txtSteamAppId);
            Controls.Add(label5);
            Controls.Add(btnPickShortcut);
            Controls.Add(txtUrl);
            Controls.Add(label4);
            Controls.Add(txtExe);
            Controls.Add(label3);
            Controls.Add(txtSteamName);
            Controls.Add(label2);
            Controls.Add(txtDisplayName);
            Controls.Add(label1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AddNonSteamToSteam";
            titleBar.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private TextBox txtDisplayName;
        private Label label2;
        private TextBox txtSteamName;
        private Label label3;
        private TextBox txtExe;
        private Label label4;
        private TextBox txtUrl;
        private Button btnPickShortcut;
        private Label label5;
        private TextBox txtSteamAppId;
        private Button btnAddUpdate;
        private Panel titleBar;
        private Button btnMin;
        private Button btnClose;
        private RadioButton rbUri;
        private RadioButton rbExe;
    }
}
