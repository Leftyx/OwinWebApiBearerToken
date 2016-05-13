using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinWebApiBearerToken
{
    public class ImageProcessingMiddleware : OwinMiddleware
    {
        public ImageProcessingMiddleware(OwinMiddleware next): base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            var username = context.Request.User.Identity.Name;

            Console.WriteLine("Begin Request");
            await Next.Invoke(context);
            Console.WriteLine("End Request");
        }
    }
}
