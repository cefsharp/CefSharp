namespace CefUsageTests
{
    partial class MainForm
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
            this.panel = new System.Windows.Forms.Panel();
            this.Test1Button = new System.Windows.Forms.Button();
            this.Test2Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel.Location = new System.Drawing.Point(12, 106);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(556, 355);
            this.panel.TabIndex = 0;
            // 
            // Test1Button
            // 
            this.Test1Button.Location = new System.Drawing.Point(12, 12);
            this.Test1Button.Name = "Test1Button";
            this.Test1Button.Size = new System.Drawing.Size(75, 23);
            this.Test1Button.TabIndex = 1;
            this.Test1Button.Text = "Test1";
            this.Test1Button.UseVisualStyleBackColor = true;
            this.Test1Button.Click += new System.EventHandler(this.Test1Button_Click);
            // 
            // Test2Button
            // 
            this.Test2Button.Location = new System.Drawing.Point(93, 12);
            this.Test2Button.Name = "Test2Button";
            this.Test2Button.Size = new System.Drawing.Size(75, 23);
            this.Test2Button.TabIndex = 2;
            this.Test2Button.Text = "Test2";
            this.Test2Button.UseVisualStyleBackColor = true;
            this.Test2Button.Click += new System.EventHandler(this.Test2Button_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 473);
            this.Controls.Add(this.Test2Button);
            this.Controls.Add(this.Test1Button);
            this.Controls.Add(this.panel);
            this.Name = "MainForm";
            this.Text = "Cef Usage Tests";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button Test1Button;
        private System.Windows.Forms.Button Test2Button;
    }
}

