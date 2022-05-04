// Copyright © 2014 The CefSharp Authors. All rights reserved.
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
        /// Source is some other "explicit" navigation. This is the default value for
        /// navigations where the actual type is unknown. See also <see cref="DirectLoad"/>.
        /// </summary>
        Explicit = 1,

        /// <summary>
		/// User got to this page through a suggestion in the UI (for example, via the
		/// destinations page). Chrome runtime only.
		/// </summary>
		AutoBookmark = 2,

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
		/// User got to this page by typing in the URL bar and selecting an entry
		/// that did not look like a URL.  For example, a match might have the URL
		/// of a Google search result page, but appear like &quot;Search Google for ...&quot;.
		/// These are not quite the same as EXPLICIT navigations because the user
		/// didn&apos;t type or see the destination URL. Chrome runtime only.
		/// See also TT_KEYWORD.
		/// </summary>
		Generated = 5,

        /// <summary>
		/// This is a toplevel navigation. This is any content that is automatically
		/// loaded in a toplevel frame.  For example, opening a tab to show the ASH
		/// screen saver, opening the devtools window, opening the NTP after the safe
		/// browsing warning, opening web-based dialog boxes are examples of
		/// AUTO_TOPLEVEL navigations. Chrome runtime only.
		/// </summary>
		AutoToplevel = 6,

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
		/// The url was generated from a replaceable keyword other than the default
		/// search provider. If the user types a keyword (which also applies to
		/// tab-to-search) in the omnibox this qualifier is applied to the transition
		/// type of the generated url. TemplateURLModel then may generate an
		/// additional visit with a transition type of TT_KEYWORD_GENERATED against the
		/// url &apos;http://&apos; + keyword. For example, if you do a tab-to-search against
		/// wikipedia the generated url has a transition qualifer of TT_KEYWORD, and
		/// TemplateURLModel generates a visit for &apos;wikipedia.org&apos; with a transition
		/// type of TT_KEYWORD_GENERATED. Chrome runtime only.
		/// </summary>
		Keyword = 9,

        /// <summary>
		/// Corresponds to a visit generated for a keyword. See description of
		/// TT_KEYWORD for more details. Chrome runtime only.
		/// </summary>
		KeywordGenerated = 10,

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
        /// Will be ORed to the transition type for the original load.
        /// </summary>
        ForwardBack = 0x01000000,

        /// <summary>
        /// Loaded a URL directly via CreateBrowser, LoadURL or LoadRequest.
        /// </summary>
        DirectLoad = 0x02000000,

        /// <summary>
		/// User is navigating to the home page. Chrome runtime only.
		/// </summary>
		HomePage = 0x04000000,

        /// <summary>
        /// The transition originated from an external application; the exact
        /// definition of this is embedder dependent. Chrome runtime and
        /// extension system only.
        /// </summary>
        FromApi = 0x08000000,

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
