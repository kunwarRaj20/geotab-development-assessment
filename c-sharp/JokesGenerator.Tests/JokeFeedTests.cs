using JokesGenerator.DataAccess.Jokes;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace JokesGenerator.Tests
{
    [TestFixture]
    public class JokeFeedTests
    {
        private ILogger<JokesFeed> _logger;

        private JokesFeed _jokesFeed;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<JokesFeed>>();
            var loggerFactory = Substitute.For<ILoggerFactory>();
            loggerFactory.CreateLogger(Arg.Any<string>()).Returns(_logger);
            _jokesFeed = new JokesFeed(_logger);
        }

        [Test]
        public async Task GetCategories()
        {
            //// Act
            var result = await _jokesFeed.GetCategories();

            //// Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public async Task GetRandomJokes_ValidCategory()
        {
            //Arrange 
            string category = "sport";
            int numberOfJokes = 2;
            //// Act
            var result = await _jokesFeed.GetRandomJokesData(category, numberOfJokes);

            //// Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public async Task GetRandomJokes_InValidCategory()
        {
            //Arrange 
            string category = "hello";
            int numberOfJokes = 3;
            //// Act
            var result = await _jokesFeed.GetRandomJokesData(category, numberOfJokes);

            //// Assert
            Assert.IsNull(result);

        }
    }
}