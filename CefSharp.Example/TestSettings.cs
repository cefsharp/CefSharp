using CefSharp;
using NUnit.Framework;

namespace CefSharp.Example
{
    [TestFixture]
    public class TestSettings
    {
        private Settings _settings;

        [SetUp]
        public void SetUp()
        {
            _settings = new Settings();
        }

        /*
        [Test]
        public void MultiThreadedMessageLoop()
        {
            Assert.That(_settings.MultiThreadedMessageLoop, Is.False);
            _settings.MultiThreadedMessageLoop = true;
            Assert.That(_settings.MultiThreadedMessageLoop, Is.True);
            _settings.MultiThreadedMessageLoop = false;
            Assert.That(_settings.MultiThreadedMessageLoop, Is.False);
        }
         * */

        [Test]
        public void CachePath()
        {
            Assert.That(_settings.CachePath, Is.Empty);
            _settings.CachePath = "SomePath";
            Assert.That(_settings.CachePath, Is.EqualTo("SomePath"));
            _settings.CachePath = null;
            Assert.That(_settings.CachePath, Is.Empty, "setting null should result in an empty string");
        }

        [Test]
        public void Locale()
        {
            Assert.That(_settings.Locale, Is.Empty);
            _settings.Locale = "Locale";
            Assert.That(_settings.Locale, Is.EqualTo("Locale"));
            _settings.Locale = null;
            Assert.That(_settings.Locale, Is.Empty, "setting null should result in an empty string");
        }

        [Test]
        public void UserAgent()
        {
            Assert.That(_settings.UserAgent, Is.Empty);
            _settings.UserAgent = "UserAgent";
            Assert.That(_settings.UserAgent, Is.EqualTo("UserAgent"));
            _settings.UserAgent = null;
            Assert.That(_settings.UserAgent, Is.Empty, "setting null should result in an empty string");
        }

        [Test]
        public void ProductVersion()
        {
            Assert.That(_settings.ProductVersion, Is.Empty);
            _settings.ProductVersion = "ProductVersion";
            Assert.That(_settings.ProductVersion, Is.EqualTo("ProductVersion"));
            _settings.ProductVersion = null;
            Assert.That(_settings.ProductVersion, Is.Empty, "setting null should result in an empty string");
        }
    }
}