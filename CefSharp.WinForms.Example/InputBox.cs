// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public event EventHandler OnEvaluate
        {
            add { _evaluate.Click += value; }
            remove { _evaluate.Click -= value; }
        }

        public string Instructions
        {
            get { return _instructions.Text; }
            set { _instructions.Text = value; }
        }

        public string Result
        {
            get { return _result.Text; }
            set { _result.Text = value; }
        }

        public string Title
        {
            set { Text = "Evaluate script - " + value; }
        }

        public string Value
        {
            get { return _value.Text; }
            set { _value.Text = value; }
        }

        private void _close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
