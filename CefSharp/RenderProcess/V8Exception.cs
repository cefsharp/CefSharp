// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.RenderProcess
{
    /// <summary>
    /// Class representing a V8 exception.
    /// </summary>
    public class V8Exception
    {
        /// <summary>
        /// Returns the index within the line of the last character where the error occurred.
        /// </summary>
        /// <returns>Returns the index within the line of the last character where the error occurred.</returns>
        public int EndColumn { get; private set; }

        /// <summary>
        /// Returns the index within the script of the last character where the error occurred.
        /// </summary>
        /// <returns>Returns the index within the script of the last character where the error occurred.</returns>
        public int EndPosition { get; private set; }

        /// <summary>
        /// Returns the 1-based number of the line where the error occurred or 0 if the line number is unknown.
        /// </summary>
        /// <returns>Returns the 1-based number of the line where the error occurred or 0 if the line number is unknown.</returns>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Returns the exception message.
        /// </summary>
        /// <returns>Returns the exception message.</returns>
        public string Message { get; private set;}

        /// <summary>
        /// Returns the resource name for the script from where the function causing the error originates.
        /// </summary>
        /// <returns>Returns the resource name for the script from where the function causing the error originates.</returns>
        public string ScriptResourceName { get; private set; }

        /// <summary>
        /// Returns the line of source code that the exception occurred within.
        /// </summary>
        /// <returns>Returns the line of source code that the exception occurred within.</returns>
        public string SourceLine { get; private set; }

        /// <summary>
        /// Returns the index within the line of the first character where the error occurred.
        /// </summary>
        /// <returns>Returns the index within the line of the first character where the error occurred.</returns>
        public int StartColumn { get; private set; }

        /// <summary>
        /// Returns the index within the script of the first character where the error occurred.
        /// </summary>
        /// <returns>Returns the index within the script of the first character where the error occurred.</returns>
        public int StartPosition { get; private set; }

        public V8Exception(int endColumn,
            int endPosition,
            int lineNumber,
            string message,
            string scriptResourceName,
            string sourceLine,
            int startColumn,
            int startPosition)
        {
            EndColumn = endColumn;
            EndPosition = endPosition;
            LineNumber = lineNumber;
            Message = message;
            ScriptResourceName = scriptResourceName;
            SourceLine = sourceLine;
            StartColumn = startColumn;
            StartPosition = startPosition;
        }

    }
}

