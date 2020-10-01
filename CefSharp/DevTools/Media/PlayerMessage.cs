// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Media
{
    /// <summary>
    /// Have one type per entry in MediaLogRecord::Type
    /// Corresponds to kMessage
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PlayerMessage : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Keep in sync with MediaLogMessageLevel
        /// We are currently keeping the message level 'error' separate from the
        /// PlayerError type because right now they represent different things,
        /// this one being a DVLOG(ERROR) style log message that gets printed
        /// based on what log level is selected in the UI, and the other is a
        /// representation of a media::PipelineStatus object. Soon however we're
        /// going to be moving away from using PipelineStatus for errors and
        /// introducing a new error type which should hopefully let us integrate
        /// the error log level into the PlayerError type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("level"), IsRequired = (true))]
        public string Level
        {
            get;
            set;
        }

        /// <summary>
        /// Message
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("message"), IsRequired = (true))]
        public string Message
        {
            get;
            set;
        }
    }
}