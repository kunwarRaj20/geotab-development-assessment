using System;
using System.Threading.Tasks;

namespace JokesGenerator.Services.Jokes
{
    public interface IJokesService
    {
        Task<string[]> GetRandomJokes(Tuple<string, string> names, string category, int numberOfJokes);
        Task<string[]> GetCategories();
        bool IsValidJokeCount(int numberOfJokes);
        Task<bool> IsCategoryValid(string categoryName);
    }
}
