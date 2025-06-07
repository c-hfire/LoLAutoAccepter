namespace LoL_AutoAccept
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            toggleAutoAcceptToolStripMenuItem = new ToolStripMenuItem();
            delayToolStripMenuItem = new ToolStripMenuItem();
            delay0ToolStripMenuItem = new ToolStripMenuItem();
            delay2ToolStripMenuItem = new ToolStripMenuItem();
            delay5ToolStripMenuItem = new ToolStripMenuItem();
            delay10ToolStripMenuItem = new ToolStripMenuItem();
            startupToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            OpenConfigFolderToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            QuitToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "LoL Auto Accepter";
            notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toggleAutoAcceptToolStripMenuItem, delayToolStripMenuItem, startupToolStripMenuItem, toolStripSeparator1, OpenConfigFolderToolStripMenuItem, toolStripSeparator2, QuitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(217, 126);
            // 
            // toggleAutoAcceptToolStripMenuItem
            // 
            toggleAutoAcceptToolStripMenuItem.Name = "toggleAutoAcceptToolStripMenuItem";
            toggleAutoAcceptToolStripMenuItem.Size = new Size(216, 22);
            toggleAutoAcceptToolStripMenuItem.Text = "自動承諾 ON/OFF";
            toggleAutoAcceptToolStripMenuItem.Click += ToggleAutoAcceptToolStripMenuItem_Click;
            // 
            // delayToolStripMenuItem
            // 
            delayToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { delay0ToolStripMenuItem, delay2ToolStripMenuItem, delay5ToolStripMenuItem, delay10ToolStripMenuItem });
            delayToolStripMenuItem.Name = "delayToolStripMenuItem";
            delayToolStripMenuItem.Size = new Size(216, 22);
            delayToolStripMenuItem.Text = "承諾ディレイ秒数";
            // 
            // delay0ToolStripMenuItem
            // 
            delay0ToolStripMenuItem.Name = "delay0ToolStripMenuItem";
            delay0ToolStripMenuItem.Size = new Size(180, 22);
            delay0ToolStripMenuItem.Text = "0秒";
            delay0ToolStripMenuItem.Click += Delay0ToolStripMenuItem_Click;
            // 
            // delay2ToolStripMenuItem
            // 
            delay2ToolStripMenuItem.Name = "delay2ToolStripMenuItem";
            delay2ToolStripMenuItem.Size = new Size(180, 22);
            delay2ToolStripMenuItem.Text = "2秒";
            delay2ToolStripMenuItem.Click += Delay2ToolStripMenuItem_Click;
            // 
            // delay5ToolStripMenuItem
            // 
            delay5ToolStripMenuItem.Name = "delay5ToolStripMenuItem";
            delay5ToolStripMenuItem.Size = new Size(180, 22);
            delay5ToolStripMenuItem.Text = "5秒";
            delay5ToolStripMenuItem.Click += Delay5ToolStripMenuItem_Click;
            // 
            // delay10ToolStripMenuItem
            // 
            delay10ToolStripMenuItem.Name = "delay10ToolStripMenuItem";
            delay10ToolStripMenuItem.Size = new Size(180, 22);
            delay10ToolStripMenuItem.Text = "10秒 (非推奨)";
            delay10ToolStripMenuItem.Click += Delay10ToolStripMenuItem_Click;
            // 
            // startupToolStripMenuItem
            // 
            startupToolStripMenuItem.Name = "startupToolStripMenuItem";
            startupToolStripMenuItem.Size = new Size(216, 22);
            startupToolStripMenuItem.Text = "Windows起動時に自動起動";
            startupToolStripMenuItem.Click += StartupToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(213, 6);
            // 
            // OpenConfigFolderToolStripMenuItem
            // 
            OpenConfigFolderToolStripMenuItem.Name = "OpenConfigFolderToolStripMenuItem";
            OpenConfigFolderToolStripMenuItem.Size = new Size(216, 22);
            OpenConfigFolderToolStripMenuItem.Text = "設定フォルダを開く";
            OpenConfigFolderToolStripMenuItem.Click += OpenConfigFolderToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(213, 6);
            // 
            // QuitToolStripMenuItem
            // 
            QuitToolStripMenuItem.Name = "QuitToolStripMenuItem";
            QuitToolStripMenuItem.Size = new Size(216, 22);
            QuitToolStripMenuItem.Text = "終了";
            QuitToolStripMenuItem.Click += QuitToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem QuitToolStripMenuItem;
        private ToolStripMenuItem toggleAutoAcceptToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem delayToolStripMenuItem;
        private ToolStripMenuItem delay0ToolStripMenuItem;
        private ToolStripMenuItem delay2ToolStripMenuItem;
        private ToolStripMenuItem delay5ToolStripMenuItem;
        private ToolStripMenuItem startupToolStripMenuItem;
        private ToolStripMenuItem OpenConfigFolderToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem delay10ToolStripMenuItem;
    }
}
