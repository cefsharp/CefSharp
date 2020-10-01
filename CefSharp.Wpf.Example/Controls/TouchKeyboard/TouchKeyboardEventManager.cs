// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using static Microsoft.Windows.Input.TouchKeyboard.Rcw.InputPaneRcw;

namespace Microsoft.Windows.Input.TouchKeyboard
{
    /// <summary>
    /// Provides Win10 Touch Keyboard - Show/Hide
    /// NOTE: Documentation suggests Win10 SDK is required to compile this.
    /// https://github.com/Microsoft/WPF-Samples/blob/master/Input%20and%20Commands/TouchKeyboard/TouchKeyboardNotifier/Readme.md
    /// </summary>
    /// <remarks>
    /// Adapted from https://github.com/Microsoft/WPF-Samples/blob/master/Input%20and%20Commands/TouchKeyboard/TouchKeyboardNotifier/TouchKeyboardEventManager.cs
    /// Licensed under an MIT license see https://github.com/Microsoft/WPF-Samples/blob/master/LICENSE
    /// </remarks>
    [CLSCompliant(true)]
    internal class TouchKeyboardEventManager : IDisposable
    {
        private const string InputPaneTypeName = "Windows.UI.ViewManagement.InputPane, Windows, ContentType=WindowsRuntime";
        /// <summary>
        /// The WinRT InputPane type.
        /// </summary>
        private readonly Type inputPaneType = null;

        private IInputPaneInterop inputPaneInterop = null;

        private IInputPane2 inputPanel = null;

        private bool disposed = false;

        /// <summary>
        /// Indicates if calling the touch keyboard is supported
        /// </summary>
        private readonly bool touchKeyboardSupported = false;

        /// <summary>
        /// TouchKeyboardEventManager
        /// </summary>
        /// <param name="handle">Need the HWND for the native interop call into IInputPaneInterop</param>
        internal TouchKeyboardEventManager(IntPtr handle)
        {
            inputPaneType = Type.GetType(InputPaneTypeName);

            // Get and cast an InputPane COM instance
            inputPaneInterop = WindowsRuntimeMarshal.GetActivationFactory(inputPaneType) as IInputPaneInterop;

            touchKeyboardSupported = inputPaneInterop != null;

            if (touchKeyboardSupported)
            {
                // Get the actual input pane for this HWND
                inputPanel = inputPaneInterop.GetForWindow(handle, typeof(IInputPane2).GUID);
            }
        }

        /// <summary>
        /// Returns an instance of the InputPane
        /// </summary>
        /// <returns>The InputPane</returns>
        internal IInputPane2 GetInputPane()
        {
            if (!touchKeyboardSupported)
            {
                throw new PlatformNotSupportedException("Native access to touch keyboard APIs not supported on this OS!");
            }

            return inputPanel;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (inputPanel != null)
                    {
                        Marshal.FinalReleaseComObject(inputPanel);

                        inputPanel = null;
                    }

                    if (inputPaneInterop != null)
                    {
                        Marshal.FinalReleaseComObject(inputPaneInterop);

                        inputPaneInterop = null;
                    }
                }
            }

            disposed = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
