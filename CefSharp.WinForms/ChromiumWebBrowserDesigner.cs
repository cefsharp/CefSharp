// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms.Design;

namespace CefSharp.WinForms
{
	public class ChromiumWebBrowserDesigner : ControlDesigner
	{
		protected override void OnPaintAdornments(System.Windows.Forms.PaintEventArgs pe)
		{
			//NOTE: Removed until a better image can be found, add image as Embedded Resource and update name below
			//var assembly = Assembly.GetAssembly(typeof(ChromiumWebBrowserDesigner));

			//using (var logo = assembly.GetManifestResourceStream("CefSharp.WinForms.CefSharpLogo.png"))
			//using (var img = Image.FromStream(logo))
			//{
			//	pe.Graphics.DrawImage(img, 0, 0);
			//}

			using(var font = new Font("Arial", 16))
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
