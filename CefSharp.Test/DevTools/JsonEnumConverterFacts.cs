// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Text.Json;
using CefSharp.DevTools.Audits;
using CefSharp.DevTools.Browser;
using CefSharp.DevTools.CSS;
using CefSharp.Internals.Json;
using Xunit;

namespace CefSharp.Test.DevTools
{
    public class JsonEnumConverterFacts
    {
        [Fact]
        public void CanConvertEnumToJsonString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };

            options.Converters.Add(new JsonEnumConverterFactory());

            var expected = "\"clipboardSanitizedWrite\"";
            var actual = JsonSerializer.Serialize(PermissionType.ClipboardSanitizedWrite, options);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConvertEnumArrayToJsonString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };

            options.Converters.Add(new JsonEnumConverterFactory());

            var expected = "[\"user-agent\",\"regular\"]";
            var actual = JsonSerializer.Serialize(new StyleSheetOrigin[] { StyleSheetOrigin.UserAgent, StyleSheetOrigin.Regular }, options);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConvertNullableEnumToJsonStringNull()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };

            options.Converters.Add(new JsonEnumConverterFactory());

            var model = new MixedContentIssueDetails
            {
                InsecureURL = "Testing",
                ResourceType = null
            };

            var expected = "{\"resolutionStatus\":\"MixedContentBlocked\",\"insecureURL\":\"Testing\"}";
            var actual = JsonSerializer.Serialize(model, options);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConvertNullableEnumToJsonStringNotNull()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };

            options.Converters.Add(new JsonEnumConverterFactory());

            var model = new MixedContentIssueDetails
            {
                InsecureURL = "Testing",
                ResourceType = MixedContentResourceType.CSPReport
            };

            var expected = "{\"resourceType\":\"CSPReport\",\"resolutionStatus\":\"MixedContentBlocked\",\"insecureURL\":\"Testing\"}";
            var actual = JsonSerializer.Serialize(model, options);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanSerializeDeserializeEnumJson()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };

            options.Converters.Add(new JsonEnumConverterFactory());

            var expected = new MixedContentIssueDetails
            {
                InsecureURL = "http://testing.domain/index.html",
                Frame = new AffectedFrame
                {
                    FrameId = "123"
                },
                ResourceType = MixedContentResourceType.CSPReport
            };

            var serialize = JsonSerializer.Serialize(expected, options);
            var actual = JsonSerializer.Deserialize<MixedContentIssueDetails>(serialize, options);

            Assert.Equal(expected.InsecureURL, actual.InsecureURL);
            Assert.Equal(expected.ResourceType, actual.ResourceType);
            Assert.Equal(expected.Frame.FrameId, actual.Frame.FrameId);
        }
    }
}
