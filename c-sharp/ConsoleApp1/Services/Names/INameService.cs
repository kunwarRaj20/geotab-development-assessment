using System.Threading.Tasks;

namespace JokesGenerator.Services.Names
{
    public interface INameService
    {
        Task<dynamic> GetNames();
    }
}
