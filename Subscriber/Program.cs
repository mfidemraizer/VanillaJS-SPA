using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                 //1
            {
                x.Service<SampleService>(s =>                        //2
                {
                    s.ConstructUsing(name => new SampleService());     //3
                    s.WhenStarted(tc => tc.Start());              //4
                    s.WhenStopped(tc => tc.Stop());               //5
                });
                x.RunAsLocalSystem();                            //6

                x.SetDescription("Sample Service");        //7
                x.SetDisplayName("Sample service");                       //8
                x.SetServiceName("SampleService");                       //9
            });                     
        }
    }
}
