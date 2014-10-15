using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinWebApiBearerToken
{
    class ApplicationClient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ClientSecretHash { get; set; }
        public OAuthGrant AllowedGrant { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
