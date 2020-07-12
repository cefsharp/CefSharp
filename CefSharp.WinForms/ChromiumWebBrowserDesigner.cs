// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections;
using System.Drawing;
using System.Windows.Forms.Design;

namespace CefSharp.WinForms
{
    /// <summary>
    /// ChromiumWebBrowser Control Designer
    /// </summary>
    public class ChromiumWebBrowserDesigner : ControlDesigner
    {
        /// <summary>
        /// Receives a call when the control that the designer is managing has painted its surface so the designer can paint any additional adornments on top of the control.
        /// </summary>
        /// <param name="pe">args</param>
        protected override void OnPaintAdornments(System.Windows.Forms.PaintEventArgs pe)
        {
            //NOTE: Removed until a better image can be found, add image as Embedded Resource and update name below
            //var assembly = Assembly.GetAssembly(typeof(ChromiumWebBrowserDesigner));

            //using (var logo = assembly.GetManifestResourceStream("CefSharp.WinForms.CefSharpLogo.png"))
            //using (var img = Image.FromStream(logo))
            //{
            //	pe.Graphics.DrawImage(img, 0, 0);
            //}

            using (var font = new Font("Arial", 16))
            using (var stringFormat = new StringFormat
            {
                // Create a StringFormat object with the each line of text, and the block
                // of text centered on the page.
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                pe.Graphics.DrawString("ChromiumWebBrowser", font, Brushes.Black, pe.ClipRectangle, stringFormat);
            }

            base.OnPaintAdornments(pe);
        }

        /// <summary>
        /// Adjusts the set of properties the component exposes through a TypeDescriptor.
        /// </summary>
        /// <param name="properties">properties</param>
        protected override void PreFilterProperties(IDictionary properties)
        {
            //Remove some of the default properties from the designer
            //they don't have much meaning for the browser
            //Probably more that can be removed/tweaked
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("Text");

            properties.Remove("Font");
            properties.Remove("ForeColor");
            properties.Remove("BackColor");
            properties.Remove("RightToLeft");

            base.PreFilterProperties(properties);
        }
    }
}
