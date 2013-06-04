namespace CefSharp.WinForms.Example
{
    partial class Browser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.outputLabel = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.backButton = new System.Windows.Forms.ToolStripButton();
            this.forwardButton = new System.Windows.Forms.ToolStripButton();
            this.urlTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.goButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDevToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeDevToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testResourceLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testSchemeLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testExecuteScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testEvaluateScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testBindMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testConsoleMessageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testTooltipMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testPopupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testLoadStringMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testCookieVisitorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            this.toolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.outputLabel);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(730, 441);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.LeftToolStripPanelVisible = false;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.RightToolStripPanelVisible = false;
            this.toolStripContainer.Size = new System.Drawing.Size(730, 466);
            this.toolStripContainer.TabIndex = 0;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.outputLabel.Location = new System.Drawing.Point(0, 428);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(0, 13);
            this.outputLabel.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backButton,
            this.forwardButton,
            this.urlTextBox,
            this.goButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.Size = new System.Drawing.Size(730, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Layout += new System.Windows.Forms.LayoutEventHandler(this.HandleToolStripLayout);
            // 
            // backButton
            // 
            this.backButton.Enabled = false;
            this.backButton.Image = global::CefSharp.WinForms.Example.Properties.Resources.nav_left_green;
            this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(52, 22);
            this.backButton.Text = "Back";
            // 
            // forwardButton
            // 
            this.forwardButton.Enabled = false;
            this.forwardButton.Image = global::CefSharp.WinForms.Example.Properties.Resources.nav_right_green;
            this.forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(70, 22);
            this.forwardButton.Text = "Forward";
            // 
            // urlTextBox
            // 
            this.urlTextBox.AutoSize = false;
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(500, 25);
            this.urlTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UrlTextBoxKeyUp);
            // 
            // goButton
            // 
            this.goButton.Image = global::CefSharp.WinForms.Example.Properties.Resources.nav_plain_green;
            this.goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(42, 22);
            this.goButton.Text = "Go";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.testsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(730, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.showDevToolsMenuItem,
            this.closeDevToolsMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
            // 
            // showDevToolsMenuItem
            // 
            this.showDevToolsMenuItem.Name = "showDevToolsMenuItem";
            this.showDevToolsMenuItem.Size = new System.Drawing.Size(158, 22);
            this.showDevToolsMenuItem.Text = "Show Dev Tools";
            // 
            // closeDevToolsMenuItem
            // 
            this.closeDevToolsMenuItem.Name = "closeDevToolsMenuItem";
            this.closeDevToolsMenuItem.Size = new System.Drawing.Size(158, 22);
            this.closeDevToolsMenuItem.Text = "Close Dev Tools";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(155, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoMenuItem,
            this.redoMenuItem,
            this.toolStripMenuItem2,
            this.cutMenuItem,
            this.copyMenuItem,
            this.pasteMenuItem,
            this.deleteMenuItem,
            this.selectAllMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoMenuItem
            // 
            this.undoMenuItem.Name = "undoMenuItem";
            this.undoMenuItem.Size = new System.Drawing.Size(122, 22);
            this.undoMenuItem.Text = "Undo";
            // 
            // redoMenuItem
            // 
            this.redoMenuItem.Name = "redoMenuItem";
            this.redoMenuItem.Size = new System.Drawing.Size(122, 22);
            this.redoMenuItem.Text = "Redo";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(119, 6);
            // 
            // cutMenuItem
            // 
            this.cutMenuItem.Name = "cutMenuItem";
            this.cutMenuItem.Size = new System.Drawing.Size(122, 22);
            this.cutMenuItem.Text = "Cut";
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.Size = new System.Drawing.Size(122, 22);
            this.copyMenuItem.Text = "Copy";
            // 
            // pasteMenuItem
            // 
            this.pasteMenuItem.Name = "pasteMenuItem";
            this.pasteMenuItem.Size = new System.Drawing.Size(122, 22);
            this.pasteMenuItem.Text = "Paste";
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.Size = new System.Drawing.Size(122, 22);
            this.deleteMenuItem.Text = "Delete";
            // 
            // selectAllMenuItem
            // 
            this.selectAllMenuItem.Name = "selectAllMenuItem";
            this.selectAllMenuItem.Size = new System.Drawing.Size(122, 22);
            this.selectAllMenuItem.Text = "Select All";
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testResourceLoadMenuItem,
            this.testSchemeLoadMenuItem,
            this.testExecuteScriptMenuItem,
            this.testEvaluateScriptMenuItem,
            this.testBindMenuItem,
            this.testConsoleMessageMenuItem,
            this.testTooltipMenuItem,
            this.testPopupMenuItem,
            this.testLoadStringMenuItem,
            this.testCookieVisitorMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // testResourceLoadMenuItem
            // 
            this.testResourceLoadMenuItem.Name = "testResourceLoadMenuItem";
            this.testResourceLoadMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testResourceLoadMenuItem.Text = "Test Resource Load Handler";
            // 
            // testSchemeLoadMenuItem
            // 
            this.testSchemeLoadMenuItem.Name = "testSchemeLoadMenuItem";
            this.testSchemeLoadMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testSchemeLoadMenuItem.Text = "Test Scheme Handler";
            // 
            // testExecuteScriptMenuItem
            // 
            this.testExecuteScriptMenuItem.Name = "testExecuteScriptMenuItem";
            this.testExecuteScriptMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testExecuteScriptMenuItem.Text = "Test Execute JavaScript";
            // 
            // testEvaluateScriptMenuItem
            // 
            this.testEvaluateScriptMenuItem.Name = "testEvaluateScriptMenuItem";
            this.testEvaluateScriptMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testEvaluateScriptMenuItem.Text = "Test Evaluate JavaScript";
            // 
            // testBindMenuItem
            // 
            this.testBindMenuItem.Name = "testBindMenuItem";
            this.testBindMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testBindMenuItem.Text = "Test Bind CLR Object to JavaScript";
            // 
            // testConsoleMessageMenuItem
            // 
            this.testConsoleMessageMenuItem.Name = "testConsoleMessageMenuItem";
            this.testConsoleMessageMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testConsoleMessageMenuItem.Text = "Test Console Message";
            // 
            // testTooltipMenuItem
            // 
            this.testTooltipMenuItem.Name = "testTooltipMenuItem";
            this.testTooltipMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testTooltipMenuItem.Text = "Test Tooltip";
            // 
            // testPopupMenuItem
            // 
            this.testPopupMenuItem.Name = "testPopupMenuItem";
            this.testPopupMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testPopupMenuItem.Text = "Test Popup";
            // 
            // testLoadStringMenuItem
            // 
            this.testLoadStringMenuItem.Name = "testLoadStringMenuItem";
            this.testLoadStringMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testLoadStringMenuItem.Text = "Test Load String";
            // 
            // testCookieVisitorMenuItem
            // 
            this.testCookieVisitorMenuItem.Name = "testCookieVisitorMenuItem";
            this.testCookieVisitorMenuItem.Size = new System.Drawing.Size(254, 22);
            this.testCookieVisitorMenuItem.Text = "Test Cookie Visitor";
            // 
            // Browser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 490);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Browser";
            this.Text = "Browser";
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.ContentPanel.PerformLayout();
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton backButton;
        private System.Windows.Forms.ToolStripButton forwardButton;
        private System.Windows.Forms.ToolStripTextBox urlTextBox;
        private System.Windows.Forms.ToolStripButton goButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testResourceLoadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testExecuteScriptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testEvaluateScriptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testSchemeLoadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testBindMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testConsoleMessageMenuItem;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.ToolStripMenuItem testTooltipMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testPopupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testLoadStringMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem showDevToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeDevToolsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem testCookieVisitorMenuItem;

    }
}