using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinWebApiBearerToken
{
    using Owin;

    public static class ImageProcessingExtensions
    {
        public static IAppBuilder UseImageProcessing(this IAppBuilder app)
        {
            return app.Use<ImageProcessingMiddleware>();
        }
    }
}
