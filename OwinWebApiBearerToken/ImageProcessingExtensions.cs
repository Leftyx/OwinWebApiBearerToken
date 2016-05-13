using Owin;

namespace OwinWebApiBearerToken
{
    public static class ImageProcessingExtensions
    {
        public static IAppBuilder UseImageProcessing(this IAppBuilder app) => app.Use<ImageProcessingMiddleware>();
    }
}
