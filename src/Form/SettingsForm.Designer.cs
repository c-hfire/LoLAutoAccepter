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
        private CheckBox checkBoxAutoBan;
        private ComboBox comboBoxAutoBanChampion;

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
            checkBoxAutoBan = new CheckBox();
            comboBoxAutoBanChampion = new ComboBox();
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
            checkBoxAutoAccept.Text = "����������L���ɂ���";
            // 
            // numericUpDownDelay
            // 
            numericUpDownDelay.Location = new Point(130, 41);
            numericUpDownDelay.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDownDelay.Name = "numericUpDownDelay";
            numericUpDownDelay.Size = new Size(60, 23);
            numericUpDownDelay.TabIndex = 2;
            // 
            // checkBoxStartup
            // 
            checkBoxStartup.AutoSize = true;
            checkBoxStartup.Location = new Point(20, 70);
            checkBoxStartup.Name = "checkBoxStartup";
            checkBoxStartup.Size = new Size(168, 19);
            checkBoxStartup.TabIndex = 3;
            checkBoxStartup.Text = "Windows�N�����Ɏ����N��";
            // 
            // textBoxLoLDir
            // 
            textBoxLoLDir.Location = new Point(20, 163);
            textBoxLoLDir.Name = "textBoxLoLDir";
            textBoxLoLDir.Size = new Size(250, 23);
            textBoxLoLDir.TabIndex = 7;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Location = new Point(276, 162);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(60, 23);
            buttonBrowse.TabIndex = 8;
            buttonBrowse.Text = "�Q��...";
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(184, 241);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 11;
            buttonOK.Text = "OK";
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(265, 241);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 12;
            buttonCancel.Text = "�L�����Z��";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // labelDelay
            // 
            labelDelay.AutoSize = true;
            labelDelay.Location = new Point(20, 44);
            labelDelay.Name = "labelDelay";
            labelDelay.Size = new Size(104, 15);
            labelDelay.TabIndex = 1;
            labelDelay.Text = "�����̒x���i�b�j:";
            // 
            // labelLoLDir
            // 
            labelLoLDir.AutoSize = true;
            labelLoLDir.Location = new Point(20, 145);
            labelLoLDir.Name = "labelLoLDir";
            labelLoLDir.Size = new Size(94, 15);
            labelLoLDir.TabIndex = 6;
            labelLoLDir.Text = "LoL�C���X�g�[����:";
            // 
            // buttonOpenConfigFolder
            // 
            buttonOpenConfigFolder.Location = new Point(20, 241);
            buttonOpenConfigFolder.Name = "buttonOpenConfigFolder";
            buttonOpenConfigFolder.Size = new Size(120, 23);
            buttonOpenConfigFolder.TabIndex = 13;
            buttonOpenConfigFolder.Text = "�ݒ�t�H���_���J��";
            buttonOpenConfigFolder.Click += ButtonOpenConfigFolder_Click;
            // 
            // checkBoxAutoClose
            // 
            checkBoxAutoClose.AutoSize = true;
            checkBoxAutoClose.Location = new Point(20, 95);
            checkBoxAutoClose.Name = "checkBoxAutoClose";
            checkBoxAutoClose.Size = new Size(164, 19);
            checkBoxAutoClose.TabIndex = 4;
            checkBoxAutoClose.Text = "������A�v���������I������";
            checkBoxAutoClose.UseVisualStyleBackColor = true;
            // 
            // checkBoxDiscordRpc
            // 
            checkBoxDiscordRpc.AutoSize = true;
            checkBoxDiscordRpc.Location = new Point(20, 120);
            checkBoxDiscordRpc.Name = "checkBoxDiscordRpc";
            checkBoxDiscordRpc.Size = new Size(151, 19);
            checkBoxDiscordRpc.TabIndex = 5;
            checkBoxDiscordRpc.Text = "Discord RPC��L���ɂ���";
            checkBoxDiscordRpc.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoBan
            // 
            checkBoxAutoBan.AutoSize = true;
            checkBoxAutoBan.Location = new Point(20, 196);
            checkBoxAutoBan.Name = "checkBoxAutoBan";
            checkBoxAutoBan.Size = new Size(130, 19);
            checkBoxAutoBan.TabIndex = 10;
            checkBoxAutoBan.Text = "�����o����L���ɂ���";
            checkBoxAutoBan.UseVisualStyleBackColor = true;
            // 
            // comboBoxAutoBanChampion
            // 
            comboBoxAutoBanChampion.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAutoBanChampion.FormattingEnabled = true;
            comboBoxAutoBanChampion.Location = new Point(169, 192);
            comboBoxAutoBanChampion.Name = "comboBoxAutoBanChampion";
            comboBoxAutoBanChampion.Size = new Size(180, 23);
            comboBoxAutoBanChampion.TabIndex = 21;
            // 
            // SettingsForm
            // 
            ClientSize = new Size(370, 291);
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
            Controls.Add(comboBoxAutoBanChampion);
            Controls.Add(buttonOK);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOpenConfigFolder);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "�ݒ�";
            ((System.ComponentModel.ISupportInitialize)numericUpDownDelay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}