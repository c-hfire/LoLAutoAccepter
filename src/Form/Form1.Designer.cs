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
            settingsStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            QuitToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = Properties.Resource1.icon_color;
            notifyIcon1.Text = "LoL Auto Accepter";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += NotifyIcon1_DoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toggleAutoAcceptToolStripMenuItem, settingsStripMenuItem, toolStripSeparator1, QuitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(170, 76);
            // 
            // toggleAutoAcceptToolStripMenuItem
            // 
            toggleAutoAcceptToolStripMenuItem.Name = "toggleAutoAcceptToolStripMenuItem";
            toggleAutoAcceptToolStripMenuItem.Size = new Size(169, 22);
            toggleAutoAcceptToolStripMenuItem.Text = "自動承諾 ON/OFF";
            toggleAutoAcceptToolStripMenuItem.Click += ToggleAutoAcceptToolStripMenuItem_Click;
            // 
            // settingsStripMenuItem
            // 
            settingsStripMenuItem.Name = "settingsStripMenuItem";
            settingsStripMenuItem.Size = new Size(169, 22);
            settingsStripMenuItem.Text = "設定";
            settingsStripMenuItem.Click += SettingsStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(166, 6);
            // 
            // QuitToolStripMenuItem
            // 
            QuitToolStripMenuItem.Name = "QuitToolStripMenuItem";
            QuitToolStripMenuItem.Size = new Size(169, 22);
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
        private ToolStripMenuItem settingsStripMenuItem;
    }
}
