// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp.Wpf.Handler
{
    /// <summary>
    /// ContextMenuExecuteModel
    /// </summary>
    public class ContextMenuExecuteModel
    {
        /// <summary>
        /// Menu Command
        /// </summary>
        public CefMenuCommand MenuCommand { get; private set; }
        /// <summary>
        /// Dictioanry Suggestions
        /// </summary>
        public IList<string> DictionarySuggestions { get; private set; }
        /// <summary>
        /// X Coordinate
        /// </summary>
        public int XCoord { get; private set; }
        /// <summary>
        /// Y Coordinate
        /// </summary>
        public int YCoord { get; private set; }
        /// <summary>
        /// Selection Text
        /// </summary>
        public string SelectionText { get; private set; }
        /// <summary>
        /// Misspelled Word
        /// </summary>
        public string MisspelledWord { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="menuCommand">menu command</param>
        /// <param name="dictionarySuggestions">dictioanry suggestion</param>
        /// <param name="xCoord">x coordinate</param>
        /// <param name="yCoord">y coordinate</param>
        /// <param name="selectionText">selection text</param>
        /// <param name="misspelledWord">misspelled word</param>
        public ContextMenuExecuteModel(CefMenuCommand menuCommand, IList<string> dictionarySuggestions, int xCoord, int yCoord, string selectionText, string misspelledWord)
        {
            MenuCommand = menuCommand;
            DictionarySuggestions = dictionarySuggestions;
            XCoord = xCoord;
            YCoord = yCoord;
            SelectionText = selectionText;
            MisspelledWord = misspelledWord;
        }
    }
}
