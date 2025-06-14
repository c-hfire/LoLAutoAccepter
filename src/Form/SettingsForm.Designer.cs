namespace LoL_AutoAccept
{
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

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
            ((System.ComponentModel.ISupportInitialize)numericUpDownDelay).BeginInit();
            SuspendLayout();
            // 
            // checkBoxAutoAccept
            // 
            checkBoxAutoAccept.AutoSize = true;
            checkBoxAutoAccept.Location = new Point(20, 20);
            checkBoxAutoAccept.Name = "checkBoxAutoAccept";
            checkBoxAutoAccept.Size = new Size(135, 19);
            checkBoxAutoAccept.TabIndex = 0;
            checkBoxAutoAccept.Text = "自動承諾を有効にする";
            // 
            // numericUpDownDelay
            // 
            numericUpDownDelay.Location = new Point(128, 53);
            numericUpDownDelay.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numericUpDownDelay.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDownDelay.Name = "numericUpDownDelay";
            numericUpDownDelay.Size = new Size(60, 23);
            numericUpDownDelay.TabIndex = 2;
            // 
            // checkBoxStartup
            // 
            checkBoxStartup.AutoSize = true;
            checkBoxStartup.Location = new Point(20, 90);
            checkBoxStartup.Name = "checkBoxStartup";
            checkBoxStartup.Size = new Size(168, 19);
            checkBoxStartup.TabIndex = 3;
            checkBoxStartup.Text = "Windows起動時に自動起動";
            // 
            // checkBoxAutoClose
            // 
            checkBoxAutoClose.AutoSize = true;
            checkBoxAutoClose.Location = new Point(20, 115);
            checkBoxAutoClose.Name = "checkBoxAutoClose";
            checkBoxAutoClose.Size = new Size(220, 19);
            checkBoxAutoClose.TabIndex = 4;
            checkBoxAutoClose.Text = "承諾後アプリを自動終了する";
            checkBoxAutoClose.UseVisualStyleBackColor = true;
            // 
            // checkBoxDiscordRpc
            // 
            checkBoxDiscordRpc.AutoSize = true;
            checkBoxDiscordRpc.Location = new Point(20, 140);
            checkBoxDiscordRpc.Name = "checkBoxDiscordRpc";
            checkBoxDiscordRpc.Size = new Size(150, 19);
            checkBoxDiscordRpc.TabIndex = 5;
            checkBoxDiscordRpc.Text = "Discord RPCを有効にする";
            checkBoxDiscordRpc.UseVisualStyleBackColor = true;
            // 
            // labelDelay
            // 
            labelDelay.AutoSize = true;
            labelDelay.Location = new Point(20, 55);
            labelDelay.Name = "labelDelay";
            labelDelay.Size = new Size(104, 15);
            labelDelay.TabIndex = 1;
            labelDelay.Text = "承諾の遅延（秒）:";
            // 
            // labelLoLDir
            // 
            labelLoLDir.AutoSize = true;
            labelLoLDir.Location = new Point(20, 170);
            labelLoLDir.Name = "labelLoLDir";
            labelLoLDir.Size = new Size(94, 15);
            labelLoLDir.TabIndex = 6;
            labelLoLDir.Text = "LoLインストール先:";
            // 
            // textBoxLoLDir
            // 
            textBoxLoLDir.Location = new Point(20, 190);
            textBoxLoLDir.Name = "textBoxLoLDir";
            textBoxLoLDir.Size = new Size(250, 23);
            textBoxLoLDir.TabIndex = 7;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Location = new Point(280, 190);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(60, 23);
            buttonBrowse.TabIndex = 8;
            buttonBrowse.Text = "参照...";
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(80, 260);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 10;
            buttonOK.Text = "OK";
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(180, 260);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 11;
            buttonCancel.Text = "キャンセル";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // buttonOpenConfigFolder
            // 
            buttonOpenConfigFolder.Location = new Point(20, 225);
            buttonOpenConfigFolder.Name = "buttonOpenConfigFolder";
            buttonOpenConfigFolder.Size = new Size(120, 23);
            buttonOpenConfigFolder.TabIndex = 9;
            buttonOpenConfigFolder.Text = "設定フォルダを開く";
            buttonOpenConfigFolder.Click += ButtonOpenConfigFolder_Click;
            // 
            // SettingsForm
            // 
            ClientSize = new Size(370, 300);
            Controls.Add(checkBoxAutoAccept);
            Controls.Add(labelDelay);
            Controls.Add(numericUpDownDelay);
            Controls.Add(checkBoxStartup);
            Controls.Add(checkBoxAutoClose);
            Controls.Add(checkBoxDiscordRpc);
            Controls.Add(labelLoLDir);
            Controls.Add(textBoxLoLDir);
            Controls.Add(buttonBrowse);
            Controls.Add(buttonOK);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOpenConfigFolder);
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