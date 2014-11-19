using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Api.Redis
{
    public static class RedisClientFactory
    {
        private readonly static AutoResetEvent _multiplexerResetEvent = new AutoResetEvent(true);
        private readonly static ConnectionMultiplexer _multiplexer;

        static RedisClientFactory()
        {
            _multiplexerResetEvent.WaitOne();

            try
            {
                ConfigurationOptions options = new ConfigurationOptions();
                options.EndPoints.Add("localhost", 6379);
                options.AbortOnConnectFail = false;

                _multiplexer = ConnectionMultiplexer.Connect(options);
            }
            finally
            {
                _multiplexerResetEvent.Set();
            }
        }

        public static IDatabase GetDatabase()
        {
            return _multiplexer.GetDatabase();
        }

        public static ITransaction GetTransaction()
        {
            return GetDatabase().CreateTransaction();
        }
    }
}
