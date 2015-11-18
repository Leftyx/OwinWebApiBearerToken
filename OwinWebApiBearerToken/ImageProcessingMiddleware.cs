using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinWebApiBearerToken
{
    using Microsoft.Owin;
    using Owin;

    public class ImageProcessingMiddleware : OwinMiddleware
    {
        public ImageProcessingMiddleware(OwinMiddleware next): base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            string username = context.Request.User.Identity.Name;

            Console.WriteLine("Begin Request");
            await Next.Invoke(context);
            Console.WriteLine("End Request");
        }
    }
}
