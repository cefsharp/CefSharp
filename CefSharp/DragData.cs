// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System.Collections.Generic;

namespace CefSharp
{
    public class DragData
    {
        /// <summary>
        /// Return the name of the file being dragged out of the browser window.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Retrieve the list of file names that are being dragged into the browser window
        /// </summary>
        public IList<string> FileNames { get; set; }

        /// <summary>
        /// Return the base URL that the fragment came from. This value is used for resolving relative URLs and may be empty. 
        /// </summary>
        public string FragmentBaseUrl { get; set; }

        /// <summary>
        /// Return the text/html fragment that is being dragged. 
        /// </summary>
        public string FragmentHtml { get; set; }

        /// <summary>
        /// Return the plain text fragment that is being dragged.
        /// </summary>
        public string FragmentText { get; set; }

        /// <summary>
        /// Return the metadata, if any, associated with the link being dragged. 
        /// </summary>
        public string LinkMetaData { set; get; }

        /// <summary>
        /// Return the title associated with the link being dragged.
        /// </summary>
        public string LinkTitle { set; get; }

        /// <summary>
        /// Return the link URL that is being dragged. 
        /// </summary>
        public string LinkUrl { set; get; }

        /// <summary>
        /// Returns true if the drag data is a file.
        /// </summary>
        public bool IsFile { get; set; }

        /// <summary>
        /// Returns true if the drag data is a text or html fragment.
        /// </summary>
        public bool IsFragment { get; set; }

        /// <summary>
        /// Returns true if the drag data is a link
        /// </summary>
        public bool IsLink { get; set; }

    }
}
