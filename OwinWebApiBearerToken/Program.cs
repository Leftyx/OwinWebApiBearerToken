namespace OwinWebApiBearerToken
{
    using System;
    using System.Web.Http;
    using Owin;
    using Microsoft.Owin.Hosting;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.OAuth;
    // using System.Web.Http.Cors;

    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:8686/";

            using (WebApp.Start(uri, Startup.Configuration))
            {
                Console.WriteLine("Started listening on " + uri);
                Console.ReadLine();
                Console.WriteLine("Shutting down...");
            }
        }
    }


}
