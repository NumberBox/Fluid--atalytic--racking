
namespace FCC_Application
{
    partial class AppMainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChildFormSpace = new System.Windows.Forms.Panel();
            this.MainNavbar = new System.Windows.Forms.ToolStrip();
            this.WebInfo = new System.Windows.Forms.ToolStripButton();
            this.UserService = new System.Windows.Forms.ToolStripButton();
            this.MainNavbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChildFormSpace
            // 
            this.ChildFormSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChildFormSpace.Location = new System.Drawing.Point(0, 24);
            this.ChildFormSpace.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChildFormSpace.Name = "ChildFormSpace";
            this.ChildFormSpace.Size = new System.Drawing.Size(1231, 866);
            this.ChildFormSpace.TabIndex = 0;
            // 
            // MainNavbar
            // 
            this.MainNavbar.BackColor = System.Drawing.Color.Azure;
            this.MainNavbar.CanOverflow = false;
            this.MainNavbar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainNavbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.WebInfo,
            this.UserService});
            this.MainNavbar.Location = new System.Drawing.Point(0, 0);
            this.MainNavbar.Name = "MainNavbar";
            this.MainNavbar.Size = new System.Drawing.Size(1231, 27);
            this.MainNavbar.TabIndex = 1;
            this.MainNavbar.Text = "toolStrip1";
            // 
            // WebInfo
            // 
            this.WebInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.WebInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.WebInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WebInfo.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.WebInfo.Name = "WebInfo";
            this.WebInfo.Size = new System.Drawing.Size(101, 24);
            this.WebInfo.Text = "Веб-справка";
            this.WebInfo.Visible = false;
            // 
            // UserService
            // 
            this.UserService.AutoToolTip = false;
            this.UserService.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UserService.Name = "UserService";
            this.UserService.Size = new System.Drawing.Size(173, 24);
            this.UserService.Text = "Сменить пользователя";
            this.UserService.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // AppMainWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1231, 891);
            this.Controls.Add(this.MainNavbar);
            this.Controls.Add(this.ChildFormSpace);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(1247, 927);
            this.Name = "AppMainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FCC";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.AppMainWindow_Activated);
            this.Deactivate += new System.EventHandler(this.AppMainWindow_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AppMainWindow_FormClosing);
            this.Load += new System.EventHandler(this.AppMainWindow_Load);
            this.MainNavbar.ResumeLayout(false);
            this.MainNavbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ChildFormSpace;
        private System.Windows.Forms.ToolStrip MainNavbar;
        private System.Windows.Forms.ToolStripButton UserService;
        private System.Windows.Forms.ToolStripButton WebInfo;
    }
}