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
            ((System.ComponentModel.ISupportInitialize)numericUpDownDelay).BeginInit();
            SuspendLayout();
            // 
            // checkBoxAutoAccept
            // 
            checkBoxAutoAccept.AutoSize = true;
            checkBoxAutoAccept.Location = new Point(24, 20);
            checkBoxAutoAccept.Name = "checkBoxAutoAccept";
            checkBoxAutoAccept.Size = new Size(150, 19);
            checkBoxAutoAccept.TabIndex = 0;
            checkBoxAutoAccept.Text = "自動承諾を有効にする";
            // 
            // labelDelay
            // 
            labelDelay.AutoSize = true;
            labelDelay.Location = new Point(24, 50);
            labelDelay.Name = "labelDelay";
            labelDelay.Size = new Size(110, 15);
            labelDelay.TabIndex = 1;
            labelDelay.Text = "承諾の遅延（秒）:";
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
            checkBoxStartup.Size = new Size(180, 19);
            checkBoxStartup.TabIndex = 3;
            checkBoxStartup.Text = "Windows起動時に自動起動";
            // 
            // checkBoxAutoClose
            // 
            checkBoxAutoClose.AutoSize = true;
            checkBoxAutoClose.Location = new Point(24, 105);
            checkBoxAutoClose.Name = "checkBoxAutoClose";
            checkBoxAutoClose.Size = new Size(180, 19);
            checkBoxAutoClose.TabIndex = 4;
            checkBoxAutoClose.Text = "承諾後アプリを自動終了する";
            checkBoxAutoClose.UseVisualStyleBackColor = true;
            // 
            // checkBoxDiscordRpc
            // 
            checkBoxDiscordRpc.AutoSize = true;
            checkBoxDiscordRpc.Location = new Point(24, 130);
            checkBoxDiscordRpc.Name = "checkBoxDiscordRpc";
            checkBoxDiscordRpc.Size = new Size(170, 19);
            checkBoxDiscordRpc.TabIndex = 5;
            checkBoxDiscordRpc.Text = "Discord RPCを有効にする";
            checkBoxDiscordRpc.UseVisualStyleBackColor = true;
            // 
            // labelLoLDir
            // 
            labelLoLDir.AutoSize = true;
            labelLoLDir.Location = new Point(24, 165);
            labelLoLDir.Name = "labelLoLDir";
            labelLoLDir.Size = new Size(110, 15);
            labelLoLDir.TabIndex = 6;
            labelLoLDir.Text = "LoLインストール先:";
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
            buttonBrowse.Location = new Point(270, 184);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(70, 25);
            buttonBrowse.TabIndex = 8;
            buttonBrowse.Text = "参照...";
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // checkBoxAutoBan
            // 
            checkBoxAutoBan.AutoSize = true;
            checkBoxAutoBan.Location = new Point(24, 220);
            checkBoxAutoBan.Name = "checkBoxAutoBan";
            checkBoxAutoBan.Size = new Size(150, 19);
            checkBoxAutoBan.TabIndex = 10;
            checkBoxAutoBan.Text = "自動バンを有効にする";
            checkBoxAutoBan.UseVisualStyleBackColor = true;
            // 
            // labelAutoBanTop
            // 
            labelAutoBanTop.AutoSize = true;
            labelAutoBanTop.Location = new Point(40, 250);
            labelAutoBanTop.Name = "labelAutoBanTop";
            labelAutoBanTop.Size = new Size(48, 15);
            labelAutoBanTop.TabIndex = 22;
            labelAutoBanTop.Text = "TOPバン";
            // 
            // comboBoxAutoBanTop
            // 
            comboBoxAutoBanTop.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanTop.FormattingEnabled = true;
            comboBoxAutoBanTop.Location = new Point(120, 246);
            comboBoxAutoBanTop.Name = "comboBoxAutoBanTop";
            comboBoxAutoBanTop.Size = new Size(150, 23);
            comboBoxAutoBanTop.TabIndex = 23;
            // 
            // labelAutoBanJungle
            // 
            labelAutoBanJungle.AutoSize = true;
            labelAutoBanJungle.Location = new Point(40, 280);
            labelAutoBanJungle.Name = "labelAutoBanJungle";
            labelAutoBanJungle.Size = new Size(38, 15);
            labelAutoBanJungle.TabIndex = 24;
            labelAutoBanJungle.Text = "JGバン";
            // 
            // comboBoxAutoBanJungle
            // 
            comboBoxAutoBanJungle.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanJungle.FormattingEnabled = true;
            comboBoxAutoBanJungle.Location = new Point(120, 276);
            comboBoxAutoBanJungle.Name = "comboBoxAutoBanJungle";
            comboBoxAutoBanJungle.Size = new Size(150, 23);
            comboBoxAutoBanJungle.TabIndex = 25;
            // 
            // labelAutoBanMid
            // 
            labelAutoBanMid.AutoSize = true;
            labelAutoBanMid.Location = new Point(40, 310);
            labelAutoBanMid.Name = "labelAutoBanMid";
            labelAutoBanMid.Size = new Size(48, 15);
            labelAutoBanMid.TabIndex = 26;
            labelAutoBanMid.Text = "MIDバン";
            // 
            // comboBoxAutoBanMid
            // 
            comboBoxAutoBanMid.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanMid.FormattingEnabled = true;
            comboBoxAutoBanMid.Location = new Point(120, 306);
            comboBoxAutoBanMid.Name = "comboBoxAutoBanMid";
            comboBoxAutoBanMid.Size = new Size(150, 23);
            comboBoxAutoBanMid.TabIndex = 27;
            // 
            // labelAutoBanAdc
            // 
            labelAutoBanAdc.AutoSize = true;
            labelAutoBanAdc.Location = new Point(40, 340);
            labelAutoBanAdc.Name = "labelAutoBanAdc";
            labelAutoBanAdc.Size = new Size(49, 15);
            labelAutoBanAdc.TabIndex = 28;
            labelAutoBanAdc.Text = "ADCバン";
            // 
            // comboBoxAutoBanAdc
            // 
            comboBoxAutoBanAdc.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanAdc.FormattingEnabled = true;
            comboBoxAutoBanAdc.Location = new Point(120, 336);
            comboBoxAutoBanAdc.Name = "comboBoxAutoBanAdc";
            comboBoxAutoBanAdc.Size = new Size(150, 23);
            comboBoxAutoBanAdc.TabIndex = 29;
            // 
            // labelAutoBanSupport
            // 
            labelAutoBanSupport.AutoSize = true;
            labelAutoBanSupport.Location = new Point(40, 370);
            labelAutoBanSupport.Name = "labelAutoBanSupport";
            labelAutoBanSupport.Size = new Size(47, 15);
            labelAutoBanSupport.TabIndex = 30;
            labelAutoBanSupport.Text = "SUPバン";
            // 
            // comboBoxAutoBanSupport
            // 
            comboBoxAutoBanSupport.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanSupport.FormattingEnabled = true;
            comboBoxAutoBanSupport.Location = new Point(120, 366);
            comboBoxAutoBanSupport.Name = "comboBoxAutoBanSupport";
            comboBoxAutoBanSupport.Size = new Size(150, 23);
            comboBoxAutoBanSupport.TabIndex = 31;
            // 
            // buttonOpenConfigFolder
            // 
            buttonOpenConfigFolder.Location = new Point(24, 410);
            buttonOpenConfigFolder.Name = "buttonOpenConfigFolder";
            buttonOpenConfigFolder.Size = new Size(130, 27);
            buttonOpenConfigFolder.TabIndex = 13;
            buttonOpenConfigFolder.Text = "設定フォルダを開く";
            buttonOpenConfigFolder.Click += ButtonOpenConfigFolder_Click;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(180, 410);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 27);
            buttonOK.TabIndex = 11;
            buttonOK.Text = "OK";
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(265, 410);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 27);
            buttonCancel.TabIndex = 12;
            buttonCancel.Text = "キャンセル";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // SettingsForm
            // 
            ClientSize = new Size(370, 460);
            Controls.Clear();
            Controls.Add(checkBoxAutoAccept);
            Controls.Add(labelDelay);
            Controls.Add(numericUpDownDelay);
            Controls.Add(checkBoxStartup);
            Controls.Add(checkBoxAutoClose);
            Controls.Add(checkBoxDiscordRpc);
            Controls.Add(labelLoLDir);
            Controls.Add(textBoxLoLDir);
            Controls.Add(buttonBrowse);
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