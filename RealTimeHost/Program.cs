using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start("http://localhost:9555"))
            {
                Console.WriteLine("Server running on 'http://localhost:9555'");
                Console.ReadLine();
            }
        }
    }
}
