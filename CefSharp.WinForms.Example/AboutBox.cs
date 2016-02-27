// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    partial class AboutBox : Form
    {
        private Assembly ExecutingAssembly { get; set; }

        public string AssemblyTitle
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length == 0)
                {
                    return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
                }
                
                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                return titleAttribute.Title != "" ? titleAttribute.Title : Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return ExecutingAssembly.GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        public AboutBox()
        {
            InitializeComponent();
            ExecutingAssembly = Assembly.GetExecutingAssembly();

            Text = "About CefTest";
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Version {0} ", Cef.CefSharpVersion);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = "CefSharp - .Net binding for Chromium\r\n\r\n"
                + "Built on Chromium Embedded Framework\r\n"
                + "   - " + Cef.CefVersion + "\r\n"
                + "Built on Chromium\r\n"
                + "   - " + Cef.ChromiumVersion + "\r\n";
        }
    }
}
