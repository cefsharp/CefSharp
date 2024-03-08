using Bogus;
using Bogus.Extensions;
using CefSharp.Internals;
using Xunit;
using Xunit.Repeat;

namespace CefSharp.Test.Framework
{
    public class JavascritpCallbackConversionTests
    {
        [Theory]
        [Repeat(1000)]
        public void CanConvertToAndFromBinary(int iteration)
        {
            var calbackFactory = new Faker<JavascriptCallback>()
                .RuleFor(o => o.Id, f => f.Random.Long(1, long.MaxValue))
                .RuleFor(o => o.BrowserId, f => f.Random.Number(1, int.MaxValue))
                .RuleFor(o => o.FrameId, f => f.Random.AlphaNumeric(120).ClampLength(120, 160));

            var exepected = calbackFactory.Generate();

            var actual = JavascriptCallback.FromBytes(exepected.ToByteArray(1));

            Assert.Equal(exepected.Id, actual.Id);
            Assert.Equal(exepected.BrowserId, actual.BrowserId);
            Assert.Equal(exepected.FrameId, actual.FrameId);

        }
    }
}
