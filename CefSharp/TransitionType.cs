// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Transition type for a request. Made up of one source value and 0 or more qualifiers.
    /// </summary>
    [Flags]
    public enum TransitionType : uint
    {
        /// <summary>
        /// Source is a link click or the JavaScript window.open function. This is
        /// also the default value for requests like sub-resource loads that are not navigations.
        /// </summary>
        LinkClicked = 0,

        /// <summary>
        /// Source is some other "explicit" navigation action such as creating a new 
        /// browser or using the LoadURL function. This is also the default value
        /// for navigations where the actual type is unknown.
        /// </summary>
        Explicit = 1,

        /// <summary>
        /// Source is a subframe navigation. This is any content that is automatically
        /// loaded in a non-toplevel frame. For example, if a page consists of several
        /// frames containing ads, those ad URLs will have this transition type.
        /// The user may not even realize the content in these pages is a separate
        /// frame, so may not care about the URL.
        /// </summary>
        AutoSubFrame = 3,

        /// <summary>
        /// Source is a subframe navigation explicitly requested by the user that will
        /// generate new navigation entries in the back/forward list. These are
        /// probably more important than frames that were automatically loaded in
        /// the background because the user probably cares about the fact that this
        /// link was loaded.
        /// </summary>
        ManualSubFrame = 4,

        /// <summary>
        /// Source is a form submission by the user. NOTE: In some situations
        /// submitting a form does not result in this transition type. This can happen
        /// if the form uses a script to submit the contents.
        /// </summary>
        FormSubmit = 7,

        /// <summary>
        /// Source is a "reload" of the page via the Reload function or by re-visiting
        /// the same URL. NOTE: This is distinct from the concept of whether a
        /// particular load uses "reload semantics" (i.e. bypasses cached data).
        /// </summary>
        Reload = 8,

        /// <summary>
        /// General mask defining the bits used for the source values.
        /// </summary>
        SourceMask = 0xFF,

        /// <summary>
        /// Attempted to visit a URL but was blocked.
        /// </summary>
        Blocked = 0x00800000,

        /// <summary>
        /// Used the Forward or Back function to navigate among browsing history.
        /// </summary>
        ForwardBack = 0x01000000,

        /// <summary>
        /// The beginning of a navigation chain.
        /// </summary>
        ChainStart = 0x10000000,

        /// <summary>
        /// The last transition in a redirect chain.
        /// </summary>
        ChainEnd = 0x20000000,

        /// <summary>
        /// Redirects caused by JavaScript or a meta refresh tag on the page.
        /// </summary>
        ClientRedirect = 0x40000000,

        /// <summary>
        /// Redirects sent from the server by HTTP headers.
        /// </summary>
        ServerRedirect = 0x80000000,

        /// <summary>
        /// Used to test whether a transition involves a redirect.
        /// </summary>
        IsRedirect = 0xC0000000,

        /// <summary>
        /// General mask defining the bits used for the qualifiers.
        /// </summary>
        QualifierMask = 0xFFFFFF00
    };
}
