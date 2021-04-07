using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JokesGenerator.DataAccess.Names
{
    public class NameFeed : INameFeed
    {
        readonly string _url = AppConsts.PrivservNameApiTypes.EndPoint;
        private readonly HttpClient _client;
        private readonly ILogger<NameFeed> _nameFeedServiceLog;
        public NameFeed(ILogger<NameFeed> nameFeedServiceLog)
        {
            _nameFeedServiceLog = nameFeedServiceLog;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_url)
            };
        }

        /// <summary>
        /// returns an object that contains name and surname
        /// </summary>
        /// <returns></returns>
		public async Task<dynamic> GetNames()
        {
            try
            {
                var data = await _client.GetStringAsync("");
                var retVal = JsonConvert.DeserializeObject<dynamic>(await _client.GetStringAsync(""));
                return retVal;
            }
            catch (Exception ex)
            {
                _nameFeedServiceLog.LogError($"NameFeed - GetNames : {ex.Message} + '\n' + {ex.StackTrace}");
                return null;
            }
        }
    }
}
