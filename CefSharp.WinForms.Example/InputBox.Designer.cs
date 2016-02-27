namespace CefSharp.WinForms.Example
{
    partial class InputBox
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
            this._instructions = new System.Windows.Forms.Label();
            this._value = new System.Windows.Forms.TextBox();
            this._evaluate = new System.Windows.Forms.Button();
            this._result = new System.Windows.Forms.TextBox();
            this._resultLabel = new System.Windows.Forms.Label();
            this._close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _instructions
            // 
            this._instructions.AutoSize = true;
            this._instructions.Location = new System.Drawing.Point(12, 19);
            this._instructions.Name = "_instructions";
            this._instructions.Size = new System.Drawing.Size(104, 13);
            this._instructions.TabIndex = 0;
            this._instructions.Text = "Enter an element ID.";
            // 
            // _value
            // 
            this._value.Location = new System.Drawing.Point(12, 35);
            this._value.Name = "_value";
            this._value.Size = new System.Drawing.Size(411, 20);
            this._value.TabIndex = 1;
            // 
            // _evaluate
            // 
            this._evaluate.Location = new System.Drawing.Point(429, 33);
            this._evaluate.Name = "_evaluate";
            this._evaluate.Size = new System.Drawing.Size(92, 23);
            this._evaluate.TabIndex = 2;
            this._evaluate.Text = "Evaluate";
            this._evaluate.UseVisualStyleBackColor = true;
            // 
            // _result
            // 
            this._result.Location = new System.Drawing.Point(58, 64);
            this._result.Name = "_result";
            this._result.ReadOnly = true;
            this._result.Size = new System.Drawing.Size(365, 20);
            this._result.TabIndex = 3;
            // 
            // _resultLabel
            // 
            this._resultLabel.AutoSize = true;
            this._resultLabel.Location = new System.Drawing.Point(12, 67);
            this._resultLabel.Name = "_resultLabel";
            this._resultLabel.Size = new System.Drawing.Size(40, 13);
            this._resultLabel.TabIndex = 4;
            this._resultLabel.Text = "Result:";
            this._resultLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _close
            // 
            this._close.Location = new System.Drawing.Point(429, 130);
            this._close.Name = "_close";
            this._close.Size = new System.Drawing.Size(92, 23);
            this._close.TabIndex = 5;
            this._close.Text = "Close";
            this._close.UseVisualStyleBackColor = true;
            this._close.Click += new System.EventHandler(this._close_Click);
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 165);
            this.Controls.Add(this._close);
            this.Controls.Add(this._resultLabel);
            this.Controls.Add(this._result);
            this.Controls.Add(this._evaluate);
            this.Controls.Add(this._value);
            this.Controls.Add(this._instructions);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Evaluate script";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _instructions;
        private System.Windows.Forms.TextBox _value;
        private System.Windows.Forms.Button _evaluate;
        private System.Windows.Forms.TextBox _result;
        private System.Windows.Forms.Label _resultLabel;
        private System.Windows.Forms.Button _close;
    }
}