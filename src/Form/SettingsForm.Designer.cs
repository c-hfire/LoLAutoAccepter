namespace LoL_AutoAccept
{
    using LoLAutoAccepter.Models;

    /// <summary>
    /// 設定フォームのデザイナークラス
    /// </summary>
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private CheckBox checkBoxAutoAccept;
        private NumericUpDown numericUpDownDelay;
        private CheckBox checkBoxStartup;
        private TextBox textBoxLoLDir;
        private Button buttonBrowse;
        private Button buttonOK;
        private Button buttonCancel;
        private Label labelDelay;
        private Label labelLoLDir;
        private Button buttonOpenConfigFolder;
        private CheckBox checkBoxAutoClose;
        private CheckBox checkBoxDiscordRpc;
        private CheckBox checkBoxAutoBan;
        private Label labelAutoBanTop;
        private ComboBox comboBoxAutoBanTop;
        private Label labelAutoBanJungle;
        private ComboBox comboBoxAutoBanJungle;
        private Label labelAutoBanMid;
        private ComboBox comboBoxAutoBanMid;
        private Label labelAutoBanAdc;
        private ComboBox comboBoxAutoBanAdc;
        private Label labelAutoBanSupport;
        private ComboBox comboBoxAutoBanSupport;

        private CheckBox checkBoxAutoPick;
        private Label labelAutoPickTop;
        private ComboBox comboBoxAutoPickTop;
        private Label labelAutoPickJungle;
        private ComboBox comboBoxAutoPickJungle;
        private Label labelAutoPickMid;
        private ComboBox comboBoxAutoPickMid;
        private Label labelAutoPickAdc;
        private ComboBox comboBoxAutoPickAdc;
        private Label labelAutoPickSupport;
        private ComboBox comboBoxAutoPickSupport;

        private ComboBox comboBoxSubPickTop;
        private ComboBox comboBoxSubPickJungle;
        private ComboBox comboBoxSubPickMid;
        private ComboBox comboBoxSubPickAdc;
        private ComboBox comboBoxSubPickSupport;

        /// <summary>
        /// デザイナーで必要なメソッド。リソースのクリーンアップを実行します。
        /// </summary>
        /// <param name="disposing">マネージドリソースを破棄する場合は true</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// コントロールの初期化
        /// </summary>
        private void InitializeComponent()
        {
            checkBoxAutoAccept = new CheckBox();
            numericUpDownDelay = new NumericUpDown();
            checkBoxStartup = new CheckBox();
            textBoxLoLDir = new TextBox();
            buttonBrowse = new Button();
            buttonOK = new Button();
            buttonCancel = new Button();
            labelDelay = new Label();
            labelLoLDir = new Label();
            buttonOpenConfigFolder = new Button();
            checkBoxAutoClose = new CheckBox();
            checkBoxDiscordRpc = new CheckBox();
            checkBoxAutoBan = new CheckBox();
            labelAutoBanTop = new Label();
            comboBoxAutoBanTop = new ComboBox();
            labelAutoBanJungle = new Label();
            comboBoxAutoBanJungle = new ComboBox();
            labelAutoBanMid = new Label();
            comboBoxAutoBanMid = new ComboBox();
            labelAutoBanAdc = new Label();
            comboBoxAutoBanAdc = new ComboBox();
            labelAutoBanSupport = new Label();
            comboBoxAutoBanSupport = new ComboBox();
            checkBoxAutoPick = new CheckBox();
            labelAutoPickTop = new Label();
            comboBoxAutoPickTop = new ComboBox();
            labelAutoPickJungle = new Label();
            comboBoxAutoPickJungle = new ComboBox();
            labelAutoPickMid = new Label();
            comboBoxAutoPickMid = new ComboBox();
            labelAutoPickAdc = new Label();
            comboBoxAutoPickAdc = new ComboBox();
            labelAutoPickSupport = new Label();
            comboBoxAutoPickSupport = new ComboBox();
            comboBoxSubPickTop = new ComboBox();
            comboBoxSubPickJungle = new ComboBox();
            comboBoxSubPickMid = new ComboBox();
            comboBoxSubPickAdc = new ComboBox();
            comboBoxSubPickSupport = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDelay).BeginInit();
            SuspendLayout();
            // 
            // checkBoxAutoAccept
            // 
            checkBoxAutoAccept.AutoSize = true;
            checkBoxAutoAccept.Location = new Point(24, 20);
            checkBoxAutoAccept.Name = "checkBoxAutoAccept";
            checkBoxAutoAccept.Size = new Size(135, 19);
            checkBoxAutoAccept.TabIndex = 0;
            checkBoxAutoAccept.Text = "自動承諾を有効にする";
            // 
            // numericUpDownDelay
            // 
            numericUpDownDelay.Location = new Point(140, 46);
            numericUpDownDelay.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDownDelay.Name = "numericUpDownDelay";
            numericUpDownDelay.Size = new Size(60, 23);
            numericUpDownDelay.TabIndex = 2;
            // 
            // checkBoxStartup
            // 
            checkBoxStartup.AutoSize = true;
            checkBoxStartup.Location = new Point(24, 80);
            checkBoxStartup.Name = "checkBoxStartup";
            checkBoxStartup.Size = new Size(168, 19);
            checkBoxStartup.TabIndex = 3;
            checkBoxStartup.Text = "Windows起動時に自動起動";
            // 
            // textBoxLoLDir
            // 
            textBoxLoLDir.Location = new Point(24, 185);
            textBoxLoLDir.Name = "textBoxLoLDir";
            textBoxLoLDir.Size = new Size(240, 23);
            textBoxLoLDir.TabIndex = 7;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Location = new Point(276, 185);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(70, 25);
            buttonBrowse.TabIndex = 8;
            buttonBrowse.Text = "参照...";
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(180, 590);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 27);
            buttonOK.TabIndex = 11;
            buttonOK.Text = "OK";
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(265, 590);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 27);
            buttonCancel.TabIndex = 12;
            buttonCancel.Text = "キャンセル";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // labelDelay
            // 
            labelDelay.AutoSize = true;
            labelDelay.Location = new Point(24, 50);
            labelDelay.Name = "labelDelay";
            labelDelay.Size = new Size(104, 15);
            labelDelay.TabIndex = 1;
            labelDelay.Text = "承諾の遅延（秒）:";
            // 
            // labelLoLDir
            // 
            labelLoLDir.AutoSize = true;
            labelLoLDir.Location = new Point(24, 165);
            labelLoLDir.Name = "labelLoLDir";
            labelLoLDir.Size = new Size(94, 15);
            labelLoLDir.TabIndex = 6;
            labelLoLDir.Text = "LoLインストール先:";
            // 
            // buttonOpenConfigFolder
            // 
            buttonOpenConfigFolder.Location = new Point(24, 590);
            buttonOpenConfigFolder.Name = "buttonOpenConfigFolder";
            buttonOpenConfigFolder.Size = new Size(130, 27);
            buttonOpenConfigFolder.TabIndex = 13;
            buttonOpenConfigFolder.Text = "設定フォルダを開く";
            buttonOpenConfigFolder.Click += ButtonOpenConfigFolder_Click;
            // 
            // checkBoxAutoClose
            // 
            checkBoxAutoClose.AutoSize = true;
            checkBoxAutoClose.Location = new Point(24, 105);
            checkBoxAutoClose.Name = "checkBoxAutoClose";
            checkBoxAutoClose.Size = new Size(164, 19);
            checkBoxAutoClose.TabIndex = 4;
            checkBoxAutoClose.Text = "承諾後アプリを自動終了する";
            checkBoxAutoClose.UseVisualStyleBackColor = true;
            // 
            // checkBoxDiscordRpc
            // 
            checkBoxDiscordRpc.AutoSize = true;
            checkBoxDiscordRpc.Location = new Point(24, 130);
            checkBoxDiscordRpc.Name = "checkBoxDiscordRpc";
            checkBoxDiscordRpc.Size = new Size(151, 19);
            checkBoxDiscordRpc.TabIndex = 5;
            checkBoxDiscordRpc.Text = "Discord RPCを有効にする";
            checkBoxDiscordRpc.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoPick
            // 
            checkBoxAutoPick.AutoSize = true;
            checkBoxAutoPick.Location = new Point(24, 220);
            checkBoxAutoPick.Name = "checkBoxAutoPick";
            checkBoxAutoPick.Size = new Size(137, 19);
            checkBoxAutoPick.TabIndex = 10;
            checkBoxAutoPick.Text = "自動ピックを有効にする";
            checkBoxAutoPick.UseVisualStyleBackColor = true;
            // 
            // labelAutoPickTop
            // 
            labelAutoPickTop.AutoSize = true;
            labelAutoPickTop.Location = new Point(40, 250);
            labelAutoPickTop.Name = "labelAutoPickTop";
            labelAutoPickTop.Size = new Size(26, 15);
            labelAutoPickTop.TabIndex = 11;
            labelAutoPickTop.Text = "TOP";
            // 
            // comboBoxAutoPickTop
            // 
            comboBoxAutoPickTop.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoPickTop.Location = new Point(100, 247);
            comboBoxAutoPickTop.Name = "comboBoxAutoPickTop";
            comboBoxAutoPickTop.Size = new Size(120, 23);
            comboBoxAutoPickTop.TabIndex = 12;
            // 
            // labelAutoPickJungle
            // 
            labelAutoPickJungle.AutoSize = true;
            labelAutoPickJungle.Location = new Point(40, 280);
            labelAutoPickJungle.Name = "labelAutoPickJungle";
            labelAutoPickJungle.Size = new Size(41, 15);
            labelAutoPickJungle.TabIndex = 13;
            labelAutoPickJungle.Text = "JG";
            // 
            // comboBoxAutoPickJungle
            // 
            comboBoxAutoPickJungle.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoPickJungle.Location = new Point(100, 277);
            comboBoxAutoPickJungle.Name = "comboBoxAutoPickJungle";
            comboBoxAutoPickJungle.Size = new Size(120, 23);
            comboBoxAutoPickJungle.TabIndex = 14;
            // 
            // labelAutoPickMid
            // 
            labelAutoPickMid.AutoSize = true;
            labelAutoPickMid.Location = new Point(40, 310);
            labelAutoPickMid.Name = "labelAutoPickMid";
            labelAutoPickMid.Size = new Size(28, 15);
            labelAutoPickMid.TabIndex = 15;
            labelAutoPickMid.Text = "MID";
            // 
            // comboBoxAutoPickMid
            // 
            comboBoxAutoPickMid.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoPickMid.Location = new Point(100, 307);
            comboBoxAutoPickMid.Name = "comboBoxAutoPickMid";
            comboBoxAutoPickMid.Size = new Size(120, 23);
            comboBoxAutoPickMid.TabIndex = 16;
            // 
            // labelAutoPickAdc
            // 
            labelAutoPickAdc.AutoSize = true;
            labelAutoPickAdc.Location = new Point(40, 340);
            labelAutoPickAdc.Name = "labelAutoPickAdc";
            labelAutoPickAdc.Size = new Size(30, 15);
            labelAutoPickAdc.TabIndex = 17;
            labelAutoPickAdc.Text = "ADC";
            // 
            // comboBoxAutoPickAdc
            // 
            comboBoxAutoPickAdc.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoPickAdc.Location = new Point(100, 337);
            comboBoxAutoPickAdc.Name = "comboBoxAutoPickAdc";
            comboBoxAutoPickAdc.Size = new Size(120, 23);
            comboBoxAutoPickAdc.TabIndex = 18;
            // 
            // labelAutoPickSupport
            // 
            labelAutoPickSupport.AutoSize = true;
            labelAutoPickSupport.Location = new Point(40, 370);
            labelAutoPickSupport.Name = "labelAutoPickSupport";
            labelAutoPickSupport.Size = new Size(49, 15);
            labelAutoPickSupport.TabIndex = 19;
            labelAutoPickSupport.Text = "SUP";
            // 
            // comboBoxAutoPickSupport
            // 
            comboBoxAutoPickSupport.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoPickSupport.Location = new Point(100, 367);
            comboBoxAutoPickSupport.Name = "comboBoxAutoPickSupport";
            comboBoxAutoPickSupport.Size = new Size(120, 23);
            comboBoxAutoPickSupport.TabIndex = 20;
            // 
            // comboBoxSubPickTop
            // 
            comboBoxSubPickTop.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSubPickTop.Location = new Point(226, 247);
            comboBoxSubPickTop.Name = "comboBoxSubPickTop";
            comboBoxSubPickTop.Size = new Size(120, 23);
            comboBoxSubPickTop.TabIndex = 21;
            // 
            // comboBoxSubPickJungle
            // 
            comboBoxSubPickJungle.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSubPickJungle.Location = new Point(226, 277);
            comboBoxSubPickJungle.Name = "comboBoxSubPickJungle";
            comboBoxSubPickJungle.Size = new Size(120, 23);
            comboBoxSubPickJungle.TabIndex = 22;
            // 
            // comboBoxSubPickMid
            // 
            comboBoxSubPickMid.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSubPickMid.Location = new Point(226, 307);
            comboBoxSubPickMid.Name = "comboBoxSubPickMid";
            comboBoxSubPickMid.Size = new Size(120, 23);
            comboBoxSubPickMid.TabIndex = 23;
            // 
            // comboBoxSubPickAdc
            // 
            comboBoxSubPickAdc.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSubPickAdc.Location = new Point(226, 337);
            comboBoxSubPickAdc.Name = "comboBoxSubPickAdc";
            comboBoxSubPickAdc.Size = new Size(120, 23);
            comboBoxSubPickAdc.TabIndex = 24;
            // 
            // comboBoxSubPickSupport
            // 
            comboBoxSubPickSupport.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSubPickSupport.Location = new Point(226, 367);
            comboBoxSubPickSupport.Name = "comboBoxSubPickSupport";
            comboBoxSubPickSupport.Size = new Size(120, 23);
            comboBoxSubPickSupport.TabIndex = 25;
            // 
            // checkBoxAutoBan
            // 
            checkBoxAutoBan.AutoSize = true;
            checkBoxAutoBan.Location = new Point(24, 400);
            checkBoxAutoBan.Name = "checkBoxAutoBan";
            checkBoxAutoBan.Size = new Size(130, 19);
            checkBoxAutoBan.TabIndex = 26;
            checkBoxAutoBan.Text = "自動バンを有効にする";
            checkBoxAutoBan.UseVisualStyleBackColor = true;
            // 
            // labelAutoBanTop
            // 
            labelAutoBanTop.AutoSize = true;
            labelAutoBanTop.Location = new Point(40, 430);
            labelAutoBanTop.Name = "labelAutoBanTop";
            labelAutoBanTop.Size = new Size(29, 15);
            labelAutoBanTop.TabIndex = 27;
            labelAutoBanTop.Text = "TOP";
            // 
            // comboBoxAutoBanTop
            // 
            comboBoxAutoBanTop.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanTop.FormattingEnabled = true;
            comboBoxAutoBanTop.Location = new Point(100, 427);
            comboBoxAutoBanTop.Name = "comboBoxAutoBanTop";
            comboBoxAutoBanTop.Size = new Size(164, 23);
            comboBoxAutoBanTop.TabIndex = 28;
            // 
            // labelAutoBanJungle
            // 
            labelAutoBanJungle.AutoSize = true;
            labelAutoBanJungle.Location = new Point(40, 460);
            labelAutoBanJungle.Name = "labelAutoBanJungle";
            labelAutoBanJungle.Size = new Size(19, 15);
            labelAutoBanJungle.TabIndex = 29;
            labelAutoBanJungle.Text = "JG";
            // 
            // comboBoxAutoBanJungle
            // 
            comboBoxAutoBanJungle.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanJungle.FormattingEnabled = true;
            comboBoxAutoBanJungle.Location = new Point(100, 457);
            comboBoxAutoBanJungle.Name = "comboBoxAutoBanJungle";
            comboBoxAutoBanJungle.Size = new Size(164, 23);
            comboBoxAutoBanJungle.TabIndex = 30;
            // 
            // labelAutoBanMid
            // 
            labelAutoBanMid.AutoSize = true;
            labelAutoBanMid.Location = new Point(40, 490);
            labelAutoBanMid.Name = "labelAutoBanMid";
            labelAutoBanMid.Size = new Size(29, 15);
            labelAutoBanMid.TabIndex = 31;
            labelAutoBanMid.Text = "MID";
            // 
            // comboBoxAutoBanMid
            // 
            comboBoxAutoBanMid.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanMid.FormattingEnabled = true;
            comboBoxAutoBanMid.Location = new Point(100, 487);
            comboBoxAutoBanMid.Name = "comboBoxAutoBanMid";
            comboBoxAutoBanMid.Size = new Size(164, 23);
            comboBoxAutoBanMid.TabIndex = 32;
            // 
            // labelAutoBanAdc
            // 
            labelAutoBanAdc.AutoSize = true;
            labelAutoBanAdc.Location = new Point(40, 520);
            labelAutoBanAdc.Name = "labelAutoBanAdc";
            labelAutoBanAdc.Size = new Size(30, 15);
            labelAutoBanAdc.TabIndex = 33;
            labelAutoBanAdc.Text = "ADC";
            // 
            // comboBoxAutoBanAdc
            // 
            comboBoxAutoBanAdc.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanAdc.FormattingEnabled = true;
            comboBoxAutoBanAdc.Location = new Point(100, 517);
            comboBoxAutoBanAdc.Name = "comboBoxAutoBanAdc";
            comboBoxAutoBanAdc.Size = new Size(164, 23);
            comboBoxAutoBanAdc.TabIndex = 34;
            // 
            // labelAutoBanSupport
            // 
            labelAutoBanSupport.AutoSize = true;
            labelAutoBanSupport.Location = new Point(40, 550);
            labelAutoBanSupport.Name = "labelAutoBanSupport";
            labelAutoBanSupport.Size = new Size(28, 15);
            labelAutoBanSupport.TabIndex = 35;
            labelAutoBanSupport.Text = "SUP";
            // 
            // comboBoxAutoBanSupport
            // 
            comboBoxAutoBanSupport.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanSupport.FormattingEnabled = true;
            comboBoxAutoBanSupport.Location = new Point(100, 547);
            comboBoxAutoBanSupport.Name = "comboBoxAutoBanSupport";
            comboBoxAutoBanSupport.Size = new Size(164, 23);
            comboBoxAutoBanSupport.TabIndex = 36;
            // 
            // SettingsForm
            // 
            ClientSize = new Size(386, 640);
            Controls.Add(checkBoxAutoAccept);
            Controls.Add(labelDelay);
            Controls.Add(numericUpDownDelay);
            Controls.Add(checkBoxStartup);
            Controls.Add(checkBoxAutoClose);
            Controls.Add(checkBoxDiscordRpc);
            Controls.Add(labelLoLDir);
            Controls.Add(textBoxLoLDir);
            Controls.Add(buttonBrowse);
            Controls.Add(checkBoxAutoPick);
            Controls.Add(labelAutoPickTop);
            Controls.Add(comboBoxAutoPickTop);
            Controls.Add(labelAutoPickJungle);
            Controls.Add(comboBoxAutoPickJungle);
            Controls.Add(labelAutoPickMid);
            Controls.Add(comboBoxAutoPickMid);
            Controls.Add(labelAutoPickAdc);
            Controls.Add(comboBoxAutoPickAdc);
            Controls.Add(labelAutoPickSupport);
            Controls.Add(comboBoxAutoPickSupport);
            Controls.Add(comboBoxSubPickTop);
            Controls.Add(comboBoxSubPickJungle);
            Controls.Add(comboBoxSubPickMid);
            Controls.Add(comboBoxSubPickAdc);
            Controls.Add(comboBoxSubPickSupport);
            Controls.Add(checkBoxAutoBan);
            Controls.Add(labelAutoBanTop);
            Controls.Add(comboBoxAutoBanTop);
            Controls.Add(labelAutoBanJungle);
            Controls.Add(comboBoxAutoBanJungle);
            Controls.Add(labelAutoBanMid);
            Controls.Add(comboBoxAutoBanMid);
            Controls.Add(labelAutoBanAdc);
            Controls.Add(comboBoxAutoBanAdc);
            Controls.Add(labelAutoBanSupport);
            Controls.Add(comboBoxAutoBanSupport);
            Controls.Add(buttonOpenConfigFolder);
            Controls.Add(buttonOK);
            Controls.Add(buttonCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "設定";
            ((System.ComponentModel.ISupportInitialize)numericUpDownDelay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}