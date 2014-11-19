using App.Api.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace App.Api
{
    [RoutePrefix("v1/products")]
    public sealed class ProductController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> GetProductsAsync()
        {
            HttpResponseMessage response = null;
            IDatabase db = RedisClientFactory.GetDatabase();

            if(await db.KeyExistsAsync("users"))
            {

            }

            return response;

        }
    }
}
