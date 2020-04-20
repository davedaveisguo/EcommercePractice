using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {

        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }


        // set cached response
        public async Task CacheReponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
           if(response ==null)
           {
               return;
           }

           var options = new JsonSerializerOptions
           {
               PropertyNamingPolicy = JsonNamingPolicy.CamelCase
           };
           var serialisedResponse = JsonSerializer.Serialize(response, options);


        // set reponse in redis and set timetoLive
           await _database.StringSetAsync(cacheKey, serialisedResponse, timeToLive);

        }


        // get cachedReponse
        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedReponse = await _database.StringGetAsync(cacheKey);

            if(cachedReponse.IsNullOrEmpty) return null;

            return cachedReponse;
        }
    }
}