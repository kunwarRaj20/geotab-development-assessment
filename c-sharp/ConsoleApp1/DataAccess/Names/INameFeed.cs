using System.Threading.Tasks;

namespace JokesGenerator.DataAccess.Names
{
    public interface INameFeed
    {
        Task<dynamic> GetNames();
    }
}
