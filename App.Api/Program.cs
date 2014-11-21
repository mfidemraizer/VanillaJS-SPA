using App.Api.Redis;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Owin.Hosting;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace App.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            const string host = "http://localhost:5454";

            using (WebApp.Start<Startup>(host))
            {
                Console.WriteLine("Listening on '{0}'", host);

                IDatabase db = RedisClientFactory.GetDatabase();
                ISubscriber subscriber = db.Multiplexer.GetSubscriber();

                subscriber.Subscribe
                (
                    "products:new",
                    async (channel, value) =>
                    {
                        using (HubConnection connection = new HubConnection("http://localhost:9555"))
                        {
                            IHubProxy productHubProxy = connection.CreateHubProxy("ProductHub");

                            await connection.Start();
                            await productHubProxy.Invoke("NotifyProductCreated", new object[] { (string)value });

                            //productHubProxy.On("productAdded", name =>
                            //{

                            //});
                        }
                    }
                );

                Console.Read();
            }
        }
    }
}
