using Microsoft.Owin.Hosting;
using System;

namespace App.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            const string host = "http://localhost:5454";

            using(WebApp.Start<Startup>(host))
            {
                Console.WriteLine("Listening on '{0}'", host);
                Console.Read();
            }
        }
    }
}
