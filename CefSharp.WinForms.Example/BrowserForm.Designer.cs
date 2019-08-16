namespace CefSharp.WinForms.Example
{
    partial class BrowserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToPdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDevToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeDevToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copySourceToClipBoardAsyncMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentZoomLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isTextInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doesElementWithIDExistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listenForButtonClickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToDemoPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.injectJavascriptCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDataUrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.httpbinorgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runFileDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadExtensionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.javascriptBindingStressTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browserTabControl = new System.Windows.Forms.TabControl();
            this.showDevToolsDockedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.zoomLevelToolStripMenuItem,
            this.scriptToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(730, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTabToolStripMenuItem,
            this.closeTabToolStripMenuItem,
            this.printToolStripMenuItem,
            this.printToPdfToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.showDevToolsMenuItem,
            this.showDevToolsDockedToolStripMenuItem,
            this.closeDevToolsMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newTabToolStripMenuItem
            // 
            this.newTabToolStripMenuItem.Name = "newTabToolStripMenuItem";
            this.newTabToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.newTabToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.newTabToolStripMenuItem.Text = "&New Tab";
            this.newTabToolStripMenuItem.Click += new System.EventHandler(this.NewTabToolStripMenuItemClick);
            // 
            // closeTabToolStripMenuItem
            // 
            this.closeTabToolStripMenuItem.Name = "closeTabToolStripMenuItem";
            this.closeTabToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeTabToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.closeTabToolStripMenuItem.Text = "&Close Tab";
            this.closeTabToolStripMenuItem.Click += new System.EventHandler(this.CloseTabToolStripMenuItemClick);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.printToolStripMenuItem.Text = "&Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintToolStripMenuItemClick);
            // 
            // printToPdfToolStripMenuItem
            // 
            this.printToPdfToolStripMenuItem.Name = "printToPdfToolStripMenuItem";
            this.printToPdfToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.printToPdfToolStripMenuItem.Text = "Print To Pdf";
            this.printToPdfToolStripMenuItem.Click += new System.EventHandler(this.PrintToPdfToolStripMenuItemClick);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
            // 
            // showDevToolsMenuItem
            // 
            this.showDevToolsMenuItem.Name = "showDevToolsMenuItem";
            this.showDevToolsMenuItem.Size = new System.Drawing.Size(208, 22);
            this.showDevToolsMenuItem.Text = "Show Dev Tools (Default)";
            this.showDevToolsMenuItem.Click += new System.EventHandler(this.ShowDevToolsMenuItemClick);
            // 
            // closeDevToolsMenuItem
            // 
            this.closeDevToolsMenuItem.Name = "closeDevToolsMenuItem";
            this.closeDevToolsMenuItem.Size = new System.Drawing.Size(208, 22);
            this.closeDevToolsMenuItem.Text = "Close Dev Tools";
            this.closeDevToolsMenuItem.Click += new System.EventHandler(this.CloseDevToolsMenuItemClick);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(205, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuItemClick);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoMenuItem,
            this.redoMenuItem,
            this.findMenuItem,
            this.toolStripMenuItem2,
            this.cutMenuItem,
            this.copyMenuItem,
            this.pasteMenuItem,
            this.deleteMenuItem,
            this.selectAllMenuItem,
            this.toolStripSeparator1,
            this.copySourceToClipBoardAsyncMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoMenuItem
            // 
            this.undoMenuItem.Name = "undoMenuItem";
            this.undoMenuItem.Size = new System.Drawing.Size(251, 22);
            this.undoMenuItem.Text = "Undo";
            this.undoMenuItem.Click += new System.EventHandler(this.UndoMenuItemClick);
            // 
            // redoMenuItem
            // 
            this.redoMenuItem.Name = "redoMenuItem";
            this.redoMenuItem.Size = new System.Drawing.Size(251, 22);
            this.redoMenuItem.Text = "Redo";
            this.redoMenuItem.Click += new System.EventHandler(this.RedoMenuItemClick);
            // 
            // findMenuItem
            // 
            this.findMenuItem.Name = "findMenuItem";
            this.findMenuItem.Size = new System.Drawing.Size(251, 22);
            this.findMenuItem.Text = "Find";
            this.findMenuItem.Click += new System.EventHandler(this.FindMenuItemClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(248, 6);
            // 
            // cutMenuItem
            // 
            this.cutMenuItem.Name = "cutMenuItem";
            this.cutMenuItem.Size = new System.Drawing.Size(251, 22);
            this.cutMenuItem.Text = "Cut";
            this.cutMenuItem.Click += new System.EventHandler(this.CutMenuItemClick);
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.Size = new System.Drawing.Size(251, 22);
            this.copyMenuItem.Text = "Copy";
            this.copyMenuItem.Click += new System.EventHandler(this.CopyMenuItemClick);
            // 
            // pasteMenuItem
            // 
            this.pasteMenuItem.Name = "pasteMenuItem";
            this.pasteMenuItem.Size = new System.Drawing.Size(251, 22);
            this.pasteMenuItem.Text = "Paste";
            this.pasteMenuItem.Click += new System.EventHandler(this.PasteMenuItemClick);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.Size = new System.Drawing.Size(251, 22);
            this.deleteMenuItem.Text = "Delete";
            this.deleteMenuItem.Click += new System.EventHandler(this.DeleteMenuItemClick);
            // 
            // selectAllMenuItem
            // 
            this.selectAllMenuItem.Name = "selectAllMenuItem";
            this.selectAllMenuItem.Size = new System.Drawing.Size(251, 22);
            this.selectAllMenuItem.Text = "Select All";
            this.selectAllMenuItem.Click += new System.EventHandler(this.SelectAllMenuItemClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(248, 6);
            // 
            // copySourceToClipBoardAsyncMenuItem
            // 
            this.copySourceToClipBoardAsyncMenuItem.Name = "copySourceToClipBoardAsyncMenuItem";
            this.copySourceToClipBoardAsyncMenuItem.Size = new System.Drawing.Size(251, 22);
            this.copySourceToClipBoardAsyncMenuItem.Text = "Copy Source to Clipboard (async)";
            this.copySourceToClipBoardAsyncMenuItem.Click += new System.EventHandler(this.CopySourceToClipBoardAsyncClick);
            // 
            // zoomLevelToolStripMenuItem
            // 
            this.zoomLevelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.currentZoomLevelToolStripMenuItem});
            this.zoomLevelToolStripMenuItem.Name = "zoomLevelToolStripMenuItem";
            this.zoomLevelToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.zoomLevelToolStripMenuItem.Text = "Zoom Level";
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.zoomInToolStripMenuItem.Text = "Zoom In";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.ZoomInToolStripMenuItemClick);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom Out";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.ZoomOutToolStripMenuItemClick);
            // 
            // currentZoomLevelToolStripMenuItem
            // 
            this.currentZoomLevelToolStripMenuItem.Name = "currentZoomLevelToolStripMenuItem";
            this.currentZoomLevelToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.currentZoomLevelToolStripMenuItem.Text = "Current Zoom Level";
            this.currentZoomLevelToolStripMenuItem.Click += new System.EventHandler(this.CurrentZoomLevelToolStripMenuItemClick);
            // 
            // scriptToolStripMenuItem
            // 
            this.scriptToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.isTextInputToolStripMenuItem,
            this.doesElementWithIDExistToolStripMenuItem,
            this.listenForButtonClickToolStripMenuItem});
            this.scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            this.scriptToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.scriptToolStripMenuItem.Text = "Script";
            // 
            // isTextInputToolStripMenuItem
            // 
            this.isTextInputToolStripMenuItem.Name = "isTextInputToolStripMenuItem";
            this.isTextInputToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.isTextInputToolStripMenuItem.Text = "Does active element accept text input";
            this.isTextInputToolStripMenuItem.Click += new System.EventHandler(this.DoesActiveElementAcceptTextInputToolStripMenuItemClick);
            // 
            // doesElementWithIDExistToolStripMenuItem
            // 
            this.doesElementWithIDExistToolStripMenuItem.Name = "doesElementWithIDExistToolStripMenuItem";
            this.doesElementWithIDExistToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.doesElementWithIDExistToolStripMenuItem.Text = "Does element with ID exist";
            this.doesElementWithIDExistToolStripMenuItem.Click += new System.EventHandler(this.DoesElementWithIdExistToolStripMenuItemClick);
            // 
            // listenForButtonClickToolStripMenuItem
            // 
            this.listenForButtonClickToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToDemoPageToolStripMenuItem,
            this.injectJavascriptCodeToolStripMenuItem});
            this.listenForButtonClickToolStripMenuItem.Name = "listenForButtonClickToolStripMenuItem";
            this.listenForButtonClickToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.listenForButtonClickToolStripMenuItem.Text = "Listen for button click";
            // 
            // goToDemoPageToolStripMenuItem
            // 
            this.goToDemoPageToolStripMenuItem.Name = "goToDemoPageToolStripMenuItem";
            this.goToDemoPageToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.goToDemoPageToolStripMenuItem.Text = "Go to demo page";
            this.goToDemoPageToolStripMenuItem.Click += new System.EventHandler(this.GoToDemoPageToolStripMenuItemClick);
            // 
            // injectJavascriptCodeToolStripMenuItem
            // 
            this.injectJavascriptCodeToolStripMenuItem.Name = "injectJavascriptCodeToolStripMenuItem";
            this.injectJavascriptCodeToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.injectJavascriptCodeToolStripMenuItem.Text = "Inject Javascript code";
            this.injectJavascriptCodeToolStripMenuItem.Click += new System.EventHandler(this.InjectJavascriptCodeToolStripMenuItemClick);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDataUrlToolStripMenuItem,
            this.httpbinorgToolStripMenuItem,
            this.runFileDialogToolStripMenuItem,
            this.loadExtensionsToolStripMenuItem,
            this.javascriptBindingStressTestToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // openDataUrlToolStripMenuItem
            // 
            this.openDataUrlToolStripMenuItem.Name = "openDataUrlToolStripMenuItem";
            this.openDataUrlToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.openDataUrlToolStripMenuItem.Text = "Open Data Url";
            this.openDataUrlToolStripMenuItem.Click += new System.EventHandler(this.OpenDataUrlToolStripMenuItemClick);
            // 
            // httpbinorgToolStripMenuItem
            // 
            this.httpbinorgToolStripMenuItem.Name = "httpbinorgToolStripMenuItem";
            this.httpbinorgToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.httpbinorgToolStripMenuItem.Text = "httpbin.org";
            this.httpbinorgToolStripMenuItem.Click += new System.EventHandler(this.OpenHttpBinOrgToolStripMenuItemClick);
            // 
            // runFileDialogToolStripMenuItem
            // 
            this.runFileDialogToolStripMenuItem.Name = "runFileDialogToolStripMenuItem";
            this.runFileDialogToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.runFileDialogToolStripMenuItem.Text = "Run File Dialog";
            this.runFileDialogToolStripMenuItem.Click += new System.EventHandler(this.RunFileDialogToolStripMenuItemClick);
            // 
            // loadExtensionsToolStripMenuItem
            // 
            this.loadExtensionsToolStripMenuItem.Name = "loadExtensionsToolStripMenuItem";
            this.loadExtensionsToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.loadExtensionsToolStripMenuItem.Text = "Load Example Extension";
            this.loadExtensionsToolStripMenuItem.Click += new System.EventHandler(this.LoadExtensionsToolStripMenuItemClick);
            // 
            // javascriptBindingStressTestToolStripMenuItem
            // 
            this.javascriptBindingStressTestToolStripMenuItem.Name = "javascriptBindingStressTestToolStripMenuItem";
            this.javascriptBindingStressTestToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.javascriptBindingStressTestToolStripMenuItem.Text = "Javascript Binding Stress Test";
            this.javascriptBindingStressTestToolStripMenuItem.Click += new System.EventHandler(this.JavascriptBindingStressTestToolStripMenuItemClick);
            // 
            // browserTabControl
            // 
            this.browserTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserTabControl.Location = new System.Drawing.Point(0, 24);
            this.browserTabControl.Name = "browserTabControl";
            this.browserTabControl.SelectedIndex = 0;
            this.browserTabControl.Size = new System.Drawing.Size(730, 466);
            this.browserTabControl.TabIndex = 2;
            // 
            // showDevToolsDockedToolStripMenuItem
            // 
            this.showDevToolsDockedToolStripMenuItem.Name = "showDevToolsDockedToolStripMenuItem";
            this.showDevToolsDockedToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.showDevToolsDockedToolStripMenuItem.Text = "Show Dev Tools (Docked)";
            this.showDevToolsDockedToolStripMenuItem.Click += new System.EventHandler(this.ShowDevToolsDockedMenuItemClick);
            // 
            // BrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 490);
            this.Controls.Add(this.browserTabControl);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BrowserForm";
            this.Text = "BrowserForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem findMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem copySourceToClipBoardAsyncMenuItem;
        private System.Windows.Forms.TabControl browserTabControl;
        private System.Windows.Forms.ToolStripMenuItem newTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeDevToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomLevelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentZoomLevelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isTextInputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doesElementWithIDExistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listenForButtonClickToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToDemoPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem injectJavascriptCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToPdfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDataUrlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem httpbinorgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runFileDialogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadExtensionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem javascriptBindingStressTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDevToolsDockedToolStripMenuItem;
    }
}
