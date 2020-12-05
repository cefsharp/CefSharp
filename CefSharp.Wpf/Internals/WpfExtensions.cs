// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CefSharp.Wpf.Internals
{
    /// <summary>
    /// Internal WpfExtension methods - unlikely you'd need to use these,
    /// they're left public on the off chance you do.
    /// </summary>
    public static class WpfExtensions
    {
        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <returns>CefEventFlags.</returns>
        public static CefEventFlags GetModifiers(this MouseEventArgs e)
        {
            CefEventFlags modifiers = 0;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                modifiers |= CefEventFlags.LeftMouseButton;
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                modifiers |= CefEventFlags.MiddleMouseButton;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                modifiers |= CefEventFlags.RightMouseButton;
            }

            modifiers |= GetModifierKeys(modifiers);

            return modifiers;
        }

        /// <summary>
        /// Gets keyboard modifiers.
        /// </summary>
        /// <returns>CefEventFlags.</returns>
        public static CefEventFlags GetModifierKeys(CefEventFlags modifiers = 0)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                modifiers |= CefEventFlags.ControlDown | CefEventFlags.IsLeft;
            }

            if (Keyboard.IsKeyDown(Key.RightCtrl))
            {
                modifiers |= CefEventFlags.ControlDown | CefEventFlags.IsRight;
            }

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                modifiers |= CefEventFlags.ShiftDown | CefEventFlags.IsLeft;
            }

            if (Keyboard.IsKeyDown(Key.RightShift))
            {
                modifiers |= CefEventFlags.ShiftDown | CefEventFlags.IsRight;
            }

            if (Keyboard.IsKeyDown(Key.LeftAlt))
            {
                modifiers |= CefEventFlags.AltDown | CefEventFlags.IsLeft;
            }

            if (Keyboard.IsKeyDown(Key.RightAlt))
            {
                modifiers |= CefEventFlags.AltDown | CefEventFlags.IsRight;
            }

            return modifiers;
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        /// <returns>CefEventFlags.</returns>
        public static CefEventFlags GetModifiers(this KeyEventArgs e)
        {
            CefEventFlags modifiers = 0;

            //Only read modifiers once for performance reasons
            //https://referencesource.microsoft.com/#PresentationCore/Core/CSharp/System/Windows/Input/KeyboardDevice.cs,227
            var keyboardDeviceModifiers = e.KeyboardDevice.Modifiers;

            if (keyboardDeviceModifiers.HasFlag(ModifierKeys.Shift))
            {
                modifiers |= CefEventFlags.ShiftDown;
            }

            if (keyboardDeviceModifiers.HasFlag(ModifierKeys.Alt))
            {
                modifiers |= CefEventFlags.AltDown;
            }

            if (keyboardDeviceModifiers.HasFlag(ModifierKeys.Control))
            {
                modifiers |= CefEventFlags.ControlDown;
            }

            return modifiers;
        }

        /// <summary>
        /// Gets the drag data wrapper.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        /// <returns>CefDragDataWrapper.</returns>
        public static IDragData GetDragData(this DragEventArgs e)
        {
            // Convert Drag Data
            var dragData = DragData.Create();

            // Files            
            dragData.IsFile = e.Data.GetDataPresent(DataFormats.FileDrop);
            if (dragData.IsFile)
            {
                // As per documentation, we only need to specify FileNames, not FileName, when dragging into the browser (http://magpcss.org/ceforum/apidocs3/projects/(default)/CefDragData.html)
                foreach (var filePath in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    var displayName = Path.GetFileName(filePath);

                    dragData.AddFile(filePath.Replace("\\", "/"), displayName);
                }
            }

            // Link/Url
            var link = GetLink(e.Data);
            dragData.IsLink = !string.IsNullOrEmpty(link);
            if (dragData.IsLink)
            {
                dragData.LinkUrl = link;
            }

            // Text/HTML
            dragData.IsFragment = e.Data.GetDataPresent(DataFormats.Text);
            if (dragData.IsFragment)
            {
                dragData.FragmentText = (string)e.Data.GetData(DataFormats.Text);

                var htmlData = (string)e.Data.GetData(DataFormats.Html);
                if (htmlData != String.Empty)
                {
                    dragData.FragmentHtml = htmlData;
                }
            }

            return dragData;
        }

        /// <summary>
        /// Gets the link.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        private static string GetLink(IDataObject data)
        {
            const string asciiUrlDataFormatName = "UniformResourceLocator";
            const string unicodeUrlDataFormatName = "UniformResourceLocatorW";

            // Try Unicode
            if (data.GetDataPresent(unicodeUrlDataFormatName))
            {
                // Try to read a Unicode URL from the data
                var unicodeUrl = ReadUrlFromDragDropData(data, unicodeUrlDataFormatName, Encoding.Unicode);
                if (unicodeUrl != null)
                {
                    return unicodeUrl;
                }
            }

            // Try ASCII
            if (data.GetDataPresent(asciiUrlDataFormatName))
            {
                // Try to read an ASCII URL from the data
                return ReadUrlFromDragDropData(data, asciiUrlDataFormatName, Encoding.ASCII);
            }

            // Not a valid link
            return null;
        }

        /// <summary>
        /// Reads a URL using a particular text encoding from drag-and-drop data.
        /// </summary>
        /// <param name="data">The drag-and-drop data.</param>
        /// <param name="urlDataFormatName">The data format name of the URL type.</param>
        /// <param name="urlEncoding">The text encoding of the URL type.</param>
        /// <returns>A URL, or <see langword="null" /> if <paramref name="data" /> does not contain a URL
        /// of the correct type.</returns>
        private static string ReadUrlFromDragDropData(IDataObject data, string urlDataFormatName, Encoding urlEncoding)
        {
            // Read the URL from the data
            string url;
            using (var urlStream = (Stream)data.GetData(urlDataFormatName))
            {
                using (TextReader reader = new StreamReader(urlStream, urlEncoding))
                {
                    url = reader.ReadToEnd();
                }
            }

            // URLs in drag/drop data are often padded with null characters so remove these
            return url.TrimEnd('\0');
        }
    }
}
