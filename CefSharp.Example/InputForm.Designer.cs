namespace CefSharp.Example
{
    partial class InputForm
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
            this.input = new System.Windows.Forms.TextBox();
            this.enterButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // input
            //
            this.input.Location = new System.Drawing.Point(12, 12);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(419, 20);
            this.input.TabIndex = 0;
            //
            // enterButton
            //
            this.enterButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.enterButton.Location = new System.Drawing.Point(437, 9);
            this.enterButton.Name = "enterButton";
            this.enterButton.Size = new System.Drawing.Size(75, 23);
            this.enterButton.TabIndex = 1;
            this.enterButton.Text = "Enter";
            this.enterButton.UseVisualStyleBackColor = true;
            //
            // InputForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 46);
            this.Controls.Add(this.enterButton);
            this.Controls.Add(this.input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Input";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.Button enterButton;
    }
}