using System;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace API.Helpers
{
    // attribute ---> httpget, httppost
    // filter allows run method before action is called  and after method executed
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }




        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // get reference to cache service
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();


            // generate a key
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);


            if (!string.IsNullOrEmpty(cachedResponse))
            {
                // here is our own context reponse send back to client
                var contentResult = new ContentResult
                {
                    // retrive this from memory
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200

                };

                context.Result = contentResult;

                return;
            }
            // if no response, send next to controller
            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.CacheReponseAsync(cacheKey, okObjectResult.Value,
                TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            // loop queryString
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}