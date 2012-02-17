namespace CefSharp.WinFormsExample
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.backButton = new System.Windows.Forms.ToolStripButton();
            this.forwardButton = new System.Windows.Forms.ToolStripButton();
            this.urlTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.goButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testResourceLoadHandlerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testRunJsSynchronouslyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testRunArbitraryJavaScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testSchemeHandlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testConsoleMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testBindCLRObjectToJSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testTooltipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bookmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cefSharpHomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fireBugLiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testPopupWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.backButton.Image = global::CefSharp.WinFormsExample.Properties.Resources.nav_left_green;
            this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(52, 22);
            this.backButton.Text = "Back";
            this.backButton.Click += new System.EventHandler(this.HandleBackButtonClick);
            // 
            // forwardButton
            // 
            this.forwardButton.Enabled = false;
            this.forwardButton.Image = global::CefSharp.WinFormsExample.Properties.Resources.nav_right_green;
            this.forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(70, 22);
            this.forwardButton.Text = "Forward";
            this.forwardButton.Click += new System.EventHandler(this.HandleForwardButtonClick);
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
            this.goButton.Image = global::CefSharp.WinFormsExample.Properties.Resources.nav_plain_green;
            this.goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(42, 22);
            this.goButton.Text = "Go";
            this.goButton.Click += new System.EventHandler(this.HandleGoButtonClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.testsToolStripMenuItem,
            this.bookmarksToolStripMenuItem});
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
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testResourceLoadHandlerMenuItem,
            this.testRunJsSynchronouslyToolStripMenuItem,
            this.testRunArbitraryJavaScriptToolStripMenuItem,
            this.testSchemeHandlerToolStripMenuItem,
            this.testConsoleMessagesToolStripMenuItem,
            this.testBindCLRObjectToJSToolStripMenuItem,
            this.testTooltipsToolStripMenuItem,
            this.testPopupWindowToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // testResourceLoadHandlerMenuItem
            // 
            this.testResourceLoadHandlerMenuItem.Name = "testResourceLoadHandlerMenuItem";
            this.testResourceLoadHandlerMenuItem.Size = new System.Drawing.Size(224, 22);
            this.testResourceLoadHandlerMenuItem.Text = "Test Resource Load Handler";
            this.testResourceLoadHandlerMenuItem.Click += new System.EventHandler(this.TestResourceLoadToolStripMenuItemClick);
            // 
            // testRunJsSynchronouslyToolStripMenuItem
            // 
            this.testRunJsSynchronouslyToolStripMenuItem.Name = "testRunJsSynchronouslyToolStripMenuItem";
            this.testRunJsSynchronouslyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.testRunJsSynchronouslyToolStripMenuItem.Text = "Test Run Js Synchronously";
            this.testRunJsSynchronouslyToolStripMenuItem.Click += new System.EventHandler(this.TestRunJsSynchronouslyToolStripMenuItemClick);
            // 
            // testRunArbitraryJavaScriptToolStripMenuItem
            // 
            this.testRunArbitraryJavaScriptToolStripMenuItem.Name = "testRunArbitraryJavaScriptToolStripMenuItem";
            this.testRunArbitraryJavaScriptToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.testRunArbitraryJavaScriptToolStripMenuItem.Text = "Test Run Arbitrary JavaScript";
            this.testRunArbitraryJavaScriptToolStripMenuItem.Click += new System.EventHandler(this.TestRunArbitraryJavaScriptToolStripMenuItemClick);
            // 
            // testSchemeHandlerToolStripMenuItem
            // 
            this.testSchemeHandlerToolStripMenuItem.Name = "testSchemeHandlerToolStripMenuItem";
            this.testSchemeHandlerToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.testSchemeHandlerToolStripMenuItem.Text = "Test Scheme Handler";
            this.testSchemeHandlerToolStripMenuItem.Click += new System.EventHandler(this.TestSchemeHandlerToolStripMenuItemClick);
            // 
            // testConsoleMessagesToolStripMenuItem
            // 
            this.testConsoleMessagesToolStripMenuItem.Name = "testConsoleMessagesToolStripMenuItem";
            this.testConsoleMessagesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.testConsoleMessagesToolStripMenuItem.Text = "Test Console Messages";
            this.testConsoleMessagesToolStripMenuItem.Click += new System.EventHandler(this.TestConsoleMessagesToolStripMenuItemClick);
            // 
            // testBindCLRObjectToJSToolStripMenuItem
            // 
            this.testBindCLRObjectToJSToolStripMenuItem.Name = "testBindCLRObjectToJSToolStripMenuItem";
            this.testBindCLRObjectToJSToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.testBindCLRObjectToJSToolStripMenuItem.Text = "Test Bind CLR object to JS";
            this.testBindCLRObjectToJSToolStripMenuItem.Click += new System.EventHandler(this.TestBindClrObjectToJsToolStripMenuItemClick);
            // 
            // testTooltipsToolStripMenuItem
            // 
            this.testTooltipsToolStripMenuItem.Name = "testTooltipsToolStripMenuItem";
            this.testTooltipsToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.testTooltipsToolStripMenuItem.Text = "Test Tooltips";
            this.testTooltipsToolStripMenuItem.Click += new System.EventHandler(this.testTooltipsToolStripMenuItem_Click);
            // 
            // bookmarksToolStripMenuItem
            // 
            this.bookmarksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cefSharpHomeToolStripMenuItem,
            this.fireBugLiteToolStripMenuItem});
            this.bookmarksToolStripMenuItem.Name = "bookmarksToolStripMenuItem";
            this.bookmarksToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.bookmarksToolStripMenuItem.Text = "Bookmarks";
            // 
            // cefSharpHomeToolStripMenuItem
            // 
            this.cefSharpHomeToolStripMenuItem.Name = "cefSharpHomeToolStripMenuItem";
            this.cefSharpHomeToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.cefSharpHomeToolStripMenuItem.Text = "CefSharp Home";
            this.cefSharpHomeToolStripMenuItem.Click += new System.EventHandler(this.cefSharpHomeToolStripMenuItem_Click);
            // 
            // fireBugLiteToolStripMenuItem
            // 
            this.fireBugLiteToolStripMenuItem.Name = "fireBugLiteToolStripMenuItem";
            this.fireBugLiteToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.fireBugLiteToolStripMenuItem.Text = "FireBug Lite";
            this.fireBugLiteToolStripMenuItem.Click += new System.EventHandler(this.fireBugLiteToolStripMenuItem_Click);
            // 
            // testPopupWindowToolStripMenuItem
            // 
            this.testPopupWindowToolStripMenuItem.Name = "testPopupWindowToolStripMenuItem";
            this.testPopupWindowToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.testPopupWindowToolStripMenuItem.Text = "Test Popup Window";
            this.testPopupWindowToolStripMenuItem.Click += new System.EventHandler(this.testPopupWindowToolStripMenuItem_Click);
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
        private System.Windows.Forms.ToolStripMenuItem testResourceLoadHandlerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testRunJsSynchronouslyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testRunArbitraryJavaScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testSchemeHandlerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testConsoleMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testBindCLRObjectToJSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bookmarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cefSharpHomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fireBugLiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testTooltipsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testPopupWindowToolStripMenuItem;

    }
}