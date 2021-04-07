using JokesGenerator.DataAccess.Names;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace JokesGenerator.Tests
{
    [TestFixture]
    public class NameFeedTests
    {
        private ILogger<NameFeed> _logger;

        private NameFeed _nameFeed;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<NameFeed>>();
            var loggerFactory = Substitute.For<ILoggerFactory>();
            loggerFactory.CreateLogger(Arg.Any<string>()).Returns(_logger);
            _nameFeed = new NameFeed(_logger);
        }

        [Test]
        public async Task GetNames()
        {
            //// Act
            var result = await _nameFeed.GetNames();

            //// Assert
            Assert.IsNotNull(result);

        }
    }
}