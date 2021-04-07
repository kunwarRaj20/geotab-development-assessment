using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JokesGenerator.DataAccess.Jokes
{
    public class JokesFeed : IJokesFeed
    {
        readonly string _url = AppConsts.ChuckNorrisApiTypes.EndPoint;
        private readonly HttpClient _client;
        private readonly ILogger<JokesFeed> _jokesFeedServiceLog;
        public JokesFeed(ILogger<JokesFeed> jokesFeedServiceLog)
        {
            _jokesFeedServiceLog = jokesFeedServiceLog;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_url)
            };
        }
        /// <summary>
        /// Get Random jokes based on preference 
        /// </summary>
        /// <param name="category">string value to specify the category of the jokes</param>
        /// <param name="numberOfJokes">integer value to specify how many jokes are required</param>
        /// <returns> a string array which contains the requested jokes</returns>
        public async Task<string[]> GetRandomJokesData(string category, int numberOfJokes)
        {
            try
            {
                int retryCounter = 0;
                string url = _url + AppConsts.ChuckNorrisApiTypes.RandomJokesAction;
                if (category != null)
                    url += AppConsts.ChuckNorrisApiTypes.JokesCategory + category;

                string[] retVal = new string[numberOfJokes];
                int counter = 0;
                do
                {
                    var jokeData = JsonConvert.DeserializeObject<dynamic>(await _client.GetStringAsync(url)).value;
                    var isValid = retVal.FirstOrDefault(t => t != null && t.Equals(jokeData.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    if (isValid == null)
                    {
                        retVal[counter] = jokeData;
                        counter++;
                    }
                    else
                        retryCounter++;

                    if (retryCounter > AppConsts.ChuckNorrisApiTypes.JokeRetryCount)
                        break;
                }
                while (counter < numberOfJokes);
                return retVal;
            }
            catch (Exception ex)
            {
                _jokesFeedServiceLog.LogError($"JokesFeed - GetRandomJokesData : {ex.Message} + '\n' + {ex.StackTrace}");
                return null;
            }
        }
        /// <summary>
        /// Gets Categories of the jokes
        /// </summary>
        /// <returns>return a string array of all the available categories</returns>
        public async Task<string[]> GetCategories()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(AppConsts.ChuckNorrisApiTypes.JokesCategoriesAction);
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());
                else
                {
                    _jokesFeedServiceLog.LogWarning("Something went wrong!! Please try again.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _jokesFeedServiceLog.LogError($"JokesFeed - GetCategories : {ex.Message} + '\n' + {ex.StackTrace}");
                return null;
            }
        }
    }
}
