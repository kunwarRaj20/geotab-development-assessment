using JokesGenerator.DataAccess.Jokes;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JokesGenerator.Services.Jokes
{
    public class JokesService : IJokesService
    {
        private readonly IJokesFeed _jokesDataFeed;
        private readonly ILogger<JokesService> _jokesServiceLog;
        public JokesService(IJokesFeed jokesDatafeed,
            ILogger<JokesService> jokesServiceLog)
        {
            _jokesDataFeed = jokesDatafeed;
            _jokesServiceLog = jokesServiceLog;
        }
        /// <summary>
        /// Validate if the number of jokes requested by the user is allowed 
        /// </summary>
        /// <param name="numberOfJokes">the number of jokes requested by the user</param>
        /// <returns></returns>
        public bool IsValidJokeCount(int numberOfJokes)
        {
            if (numberOfJokes < 1 || numberOfJokes > 9)
                return false;
            return true;
        }

        /// <summary>
        /// validates the joke count, if valid return the jokes 
        /// </summary>
        /// <param name="names"></param>
        /// <param name="category"></param>
        /// <param name="numberOfJokes"></param>
        /// <returns>string array of the jokes, if count is not valid then return empty</returns>
        public async Task<string[]> GetRandomJokes(Tuple<string, string> names, string category, int numberOfJokes)
        {
            try
            {
                if (IsValidJokeCount(numberOfJokes))
                {
                    string[] retVal = await _jokesDataFeed.GetRandomJokesData(category, numberOfJokes);

                    if (!string.IsNullOrEmpty(names?.Item1) && !string.IsNullOrEmpty(names?.Item2))
                        retVal.ReplaceAllOccurrences(AppConsts.DefaultTypes.DefaultName, names.Item1 + " " + names.Item2);

                    return retVal;
                }
                return Array.Empty<string>();
            }
            catch (Exception ex)
            {
                _jokesServiceLog.LogError($"JokesService - GetRandomJokes : {ex.Message} + '\n' + {ex.StackTrace}");
                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// returns string array of all categories 
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> GetCategories()
        {
            try
            {
                return await _jokesDataFeed.GetCategories();
            }
            catch (Exception ex)
            {
                _jokesServiceLog.LogError($"JokesService - GetCategories : {ex.Message} + '\n' + {ex.StackTrace}");
                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// validates if the category entered by user is correct 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public async Task<bool> IsCategoryValid(string categoryName)
        {
            try
            {
                var categories = await GetCategories();
                var retVal = categories.FirstOrDefault(t => t.Equals(categoryName, StringComparison.InvariantCultureIgnoreCase));
                if (retVal != null)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                _jokesServiceLog.LogError($"JokesService - IsCategoryValid : {ex.Message} + '\n' + {ex.StackTrace}");
                return false;
            }
        }
    }
}
