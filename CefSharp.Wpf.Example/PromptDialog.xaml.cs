// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;

namespace CefSharp.Wpf.Example
{
    /// <summary>
    /// PromptDialog - Basic Prompt Dialog
    /// </summary>
    public partial class PromptDialog : Window
    {
        public PromptDialog()
        {
            InitializeComponent();
        }

        public static Tuple<bool, string> Prompt(string messageText, string title, string defaultPromptText = "")
        {
            var window = new PromptDialog
            {
                Title = title
            };
            window.messageText.Text = messageText;
            window.userPrompt.Text = defaultPromptText;

            var result = window.ShowDialog();

            if (result == true)
            {
                return Tuple.Create(true, window.userPrompt.Text);
            }
            return Tuple.Create(false, string.Empty);
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
