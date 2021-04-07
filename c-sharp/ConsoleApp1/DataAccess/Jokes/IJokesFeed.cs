using System.Threading.Tasks;

namespace JokesGenerator.DataAccess.Jokes
{
    public interface IJokesFeed
    {
        Task<string[]> GetRandomJokesData(string category, int numberOfJokes);
        Task<string[]> GetCategories();
    }
}
