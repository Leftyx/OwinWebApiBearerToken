using Microsoft.Owin.Hosting;
using System;

namespace OwinWebApiBearerToken
{
    class Program
    {
        static void Main(string[] args)
        {
            const string uri = "http://localhost:8686/";

            using (WebApp.Start(uri, Startup.Configuration))
            {
                Console.WriteLine("Started listening on " + uri);
                Console.ReadLine();
                Console.WriteLine("Shutting down...");
            }
        }
    }
}
