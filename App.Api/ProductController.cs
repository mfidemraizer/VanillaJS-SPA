using App.Api.Redis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace App.Api
{
    [RoutePrefix("v1/products")]
    public sealed class ProductController : ApiController
    {
        [HttpGet, Route("")]
        public async Task<HttpResponseMessage> GetProductsAsync()
        {
            IDatabase db = RedisClientFactory.GetDatabase();

            RedisValue[] result = await db.SortAsync("products", by: "nosort", get: new RedisValue[] { "products:*" });

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent('[' + string.Join(",", result.Select(product => (string)product)) + ']');
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }

        [HttpPost, Route("")]
        public async Task<HttpResponseMessage> CreateProductAsync(dynamic args)
        {
            IDatabase db = RedisClientFactory.GetDatabase();
            ITransaction transaction = RedisClientFactory.GetTransaction();

            long newId = await db.StringIncrementAsync("products:id");

            transaction.SetAddAsync("products", newId);
            transaction.StringSetAsync("products:" + newId, (string)await JsonConvert.SerializeObjectAsync(args));

            await transaction.ExecuteAsync();

            await db.PublishAsync("products:new", (string)args.name);

            return Request.CreateResponse();
        }
    }
}
