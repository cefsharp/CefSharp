namespace CefSharp.WinForms.Example.Minimal
{
    partial class TabulationDemoForm
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
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnGO = new System.Windows.Forms.Button();
            this.grpBrowser = new System.Windows.Forms.GroupBox();
            this.txtDummy = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtURL
            // 
            this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURL.Location = new System.Drawing.Point(8, 11);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(774, 20);
            this.txtURL.TabIndex = 0;
            this.txtURL.Text = "http://www.google.com";
            this.txtURL.Enter += new System.EventHandler(this.TxtUrlEnter);
            this.txtURL.Leave += new System.EventHandler(this.TxtUrlLeave);
            // 
            // btnGO
            // 
            this.btnGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGO.Location = new System.Drawing.Point(782, 9);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(33, 23);
            this.btnGO.TabIndex = 1;
            this.btnGO.Text = "GO";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.BtnGoClick);
            // 
            // grpBrowser
            // 
            this.grpBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBrowser.Location = new System.Drawing.Point(8, 37);
            this.grpBrowser.Name = "grpBrowser";
            this.grpBrowser.Size = new System.Drawing.Size(807, 568);
            this.grpBrowser.TabIndex = 2;
            this.grpBrowser.TabStop = false;
            this.grpBrowser.Text = "Browser";
            // 
            // txtDummy
            // 
            this.txtDummy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDummy.Location = new System.Drawing.Point(8, 612);
            this.txtDummy.Name = "txtDummy";
            this.txtDummy.Size = new System.Drawing.Size(807, 20);
            this.txtDummy.TabIndex = 3;
            this.txtDummy.Text = "Dummy Text";
            // 
            // TabulationDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 642);
            this.Controls.Add(this.txtDummy);
            this.Controls.Add(this.grpBrowser);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.txtURL);
            this.Name = "TabulationDemoForm";
            this.Text = "Tabulation Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.GroupBox grpBrowser;
        private System.Windows.Forms.TextBox txtDummy;
    }
}