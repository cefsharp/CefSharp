namespace CefSharp.WinForms.Example
{
    partial class BrowserTabUserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.findTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.findPreviousButton = new System.Windows.Forms.ToolStripButton();
            this.findNextButton = new System.Windows.Forms.ToolStripButton();
            this.findCloseButton = new System.Windows.Forms.ToolStripButton();
            this.statusLabel = new System.Windows.Forms.Label();
            this.outputLabel = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.backButton = new System.Windows.Forms.ToolStripButton();
            this.forwardButton = new System.Windows.Forms.ToolStripButton();
            this.urlTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.goButton = new System.Windows.Forms.ToolStripButton();
            this.browserSplitContainer = new System.Windows.Forms.SplitContainer();
            this.browserPanel = new System.Windows.Forms.Panel();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.browserSplitContainer)).BeginInit();
            this.browserSplitContainer.Panel1.SuspendLayout();
            this.browserSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findTextBox,
            this.findPreviousButton,
            this.findNextButton,
            this.findCloseButton});
            this.toolStrip2.Location = new System.Drawing.Point(0, 465);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(730, 25);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Visible = false;
            // 
            // findTextBox
            // 
            this.findTextBox.Name = "findTextBox";
            this.findTextBox.Size = new System.Drawing.Size(100, 25);
            this.findTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindTextBoxKeyDown);
            // 
            // findPreviousButton
            // 
            this.findPreviousButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findPreviousButton.Image = global::CefSharp.WinForms.Example.Properties.Resources.nav_left_green;
            this.findPreviousButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findPreviousButton.Name = "findPreviousButton";
            this.findPreviousButton.Size = new System.Drawing.Size(23, 22);
            this.findPreviousButton.Text = "Find Previous";
            this.findPreviousButton.Click += new System.EventHandler(this.FindPreviousButtonClick);
            // 
            // findNextButton
            // 
            this.findNextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findNextButton.Image = global::CefSharp.WinForms.Example.Properties.Resources.nav_right_green;
            this.findNextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findNextButton.Name = "findNextButton";
            this.findNextButton.Size = new System.Drawing.Size(23, 22);
            this.findNextButton.Text = "Find Next";
            this.findNextButton.Click += new System.EventHandler(this.FindNextButtonClick);
            // 
            // findCloseButton
            // 
            this.findCloseButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.findCloseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.findCloseButton.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.findCloseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findCloseButton.Name = "findCloseButton";
            this.findCloseButton.Size = new System.Drawing.Size(23, 22);
            this.findCloseButton.Text = "X";
            this.findCloseButton.Click += new System.EventHandler(this.FindCloseButtonClick);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusLabel.Location = new System.Drawing.Point(0, 464);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 13);
            this.statusLabel.TabIndex = 1;
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.outputLabel.Location = new System.Drawing.Point(0, 477);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(0, 13);
            this.outputLabel.TabIndex = 0;
            // 
            // toolStrip1
            // 
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
            this.backButton.Click += new System.EventHandler(this.BackButtonClick);
            // 
            // forwardButton
            // 
            this.forwardButton.Enabled = false;
            this.forwardButton.Image = global::CefSharp.WinForms.Example.Properties.Resources.nav_right_green;
            this.forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(70, 22);
            this.forwardButton.Text = "Forward";
            this.forwardButton.Click += new System.EventHandler(this.ForwardButtonClick);
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
            this.goButton.Click += new System.EventHandler(this.GoButtonClick);
            // 
            // browserSplitContainer
            // 
            this.browserSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserSplitContainer.Location = new System.Drawing.Point(0, 25);
            this.browserSplitContainer.Name = "browserSplitContainer";
            // 
            // browserSplitContainer.Panel1
            // 
            this.browserSplitContainer.Panel1.Controls.Add(this.browserPanel);
            this.browserSplitContainer.Panel2Collapsed = true;
            this.browserSplitContainer.Size = new System.Drawing.Size(730, 439);
            this.browserSplitContainer.SplitterDistance = 481;
            this.browserSplitContainer.TabIndex = 2;
            // 
            // browserPanel
            // 
            this.browserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserPanel.Location = new System.Drawing.Point(0, 0);
            this.browserPanel.Name = "browserPanel";
            this.browserPanel.Size = new System.Drawing.Size(730, 439);
            this.browserPanel.TabIndex = 3;
            // 
            // BrowserTabUserControl
            // 
            this.Controls.Add(this.browserSplitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.toolStrip2);
            this.Name = "BrowserTabUserControl";
            this.Size = new System.Drawing.Size(730, 490);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.browserSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.browserSplitContainer)).EndInit();
            this.browserSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton backButton;
        private System.Windows.Forms.ToolStripButton forwardButton;
        private System.Windows.Forms.ToolStripTextBox urlTextBox;
        private System.Windows.Forms.ToolStripButton goButton;
        private System.Windows.Forms.Label outputLabel;

        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton findPreviousButton;
        private System.Windows.Forms.ToolStripTextBox findTextBox;
        private System.Windows.Forms.ToolStripButton findNextButton;
        private System.Windows.Forms.ToolStripButton findCloseButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.SplitContainer browserSplitContainer;
        private System.Windows.Forms.Panel browserPanel;
    }
}
