using JokesGenerator.DataAccess.Names;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JokesGenerator.Services.Names
{
    public class NameService : INameService
    {
        private readonly INameFeed _nameDataFeed;
        private readonly ILogger<NameService> _nameServiceLog;
        public NameService(INameFeed nameDataFeed,
            ILogger<NameService> nameServiceLog)
        {
            _nameDataFeed = nameDataFeed;
            _nameServiceLog = nameServiceLog;
        }

        /// <summary>
        /// returns a random name
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetNames()
        {
            try
            {
                _nameServiceLog.LogTrace("Get Names processed");
                var data = await _nameDataFeed.GetNames();
                return data;
            }
            catch (Exception ex)
            {
                _nameServiceLog.LogError($"NameService - GetNames : {ex.Message} + '\n' + {ex.StackTrace}");
                return null;
            }
        }
    }
}
