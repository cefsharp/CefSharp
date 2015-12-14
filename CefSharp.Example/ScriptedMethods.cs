// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CefSharp.Example
{
    public class ScriptedMethods
    {
        public static async Task<bool> ActiveElementAcceptsTextInput(IFrame frame)
        {
            if (frame == null)
            {
                throw new ArgumentException("An IFrame instance is required.", "frame");
            }

            // Scripts should be minified for production builds. The script
            // could also be read from a file...
            var script =
                "(function () {" +
                "	var isText = false;" +
                "	var activeElement = document.activeElement;" +
                "	if (activeElement) {" +
                "		if (activeElement.tagName.toLowerCase() === 'textarea') {" +
                "			isText = true;" +
                "		} else {" +
                "			if (activeElement.tagName.toLowerCase() === 'input') {" +
                "				if (activeElement.hasAttribute('type')) {" +
                "					var inputType = activeElement.getAttribute('type').toLowerCase();" +
                "					if (inputType === 'text' || inputType === 'email' || inputType === 'password' || inputType === 'tel' || inputType === 'number' || inputType === 'range' || inputType === 'search' || inputType === 'url') {" +
                "						isText = true;" +
                "					}" +
                "				}" +
                "			}" +
                "		}" +
                "	}" +
                "	return isText;" +
                "})();";

            var response = await frame.EvaluateScriptAsync(script);
            if (!response.Success)
            {
                throw new Exception(response.Message);
            }

            return (bool)response.Result;
        }

        public static async Task<bool> ElementWithIdExists(string id, IFrame frame)
        {
            if (frame == null)
            {
                throw new ArgumentException("An IFrame instance is required.", "frame");
            }

            var script =
                "(function () {" +
                "    var n = document.getElementById('##ID##');" +
                "    return n !== null && typeof n !== 'undefined';" +
                "})();";

            // For simple inline scripts you could use String.Format() but
            // beware of braces in the javascript code. If reading from a file
            // it's probably safer to include tokens that can be replaced via
            // regex.
            script = Regex.Replace(script, "##ID##", id);

            var response = await frame.EvaluateScriptAsync(script);
            if (!response.Success)
            {
                throw new Exception(response.Message);
            }

            return (bool)response.Result;
        }
    }
}
