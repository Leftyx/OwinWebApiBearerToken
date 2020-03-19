using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace OwinWebApiBearerToken.Providers
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public MyAuthorizationServerProvider()
        {
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var clientId = string.Empty;
            var clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.Rejected();
                context.SetError("invalid_client", "Client credentials could not be retrieved through the Authorization header.");
                return;
            }

            try
            {
                if (clientId == "MyApp" && clientSecret == "MySecret")
                {
                    var client = new ApplicationClient
                    {
                        Id = "MyApp",
                        AllowedGrant = OAuthGrant.ResourceOwner,
                        ClientSecretHash = new PasswordHasher().HashPassword("MySecret"),
                        Name = "My App",
                        CreatedOn = DateTimeOffset.UtcNow
                    };

                    context.OwinContext.Set<ApplicationClient>("oauth:client", client);

                    // string uid = context.Parameters.Where(f => f.Key == "uid").Select(f => f.Value).SingleOrDefault()[0];
                    // context.OwinContext.Set<string>("SmartCard", uid);

                    context.Validated(clientId);
                }
                else
                {
                    // Client could not be validated.
                    context.SetError("invalid_client", "Client credentials are invalid.");
                    context.Rejected();
                }
            }
            catch (Exception)
            {
                context.SetError("server_error");
                context.Rejected();
            }

            return;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var client = context.OwinContext.Get<ApplicationClient>("oauth:client");

            if (string.IsNullOrEmpty(context.UserName) || string.IsNullOrEmpty(context.Password))
            {
                context.SetError("invalid_request", "No username or password are provided.");
                context.Rejected();
                return;
            }

            if (context.UserName != "John" && context.Password != "Smith")
            {
                context.SetError("invalid_grant", "The username or password is incorrect.");
                context.Rejected();
                return;
            }

            if (client.AllowedGrant != OAuthGrant.ResourceOwner)
            {
                context.SetError("invalid_grant", "The resource owner credentials are invalid or resource owner does not exist.");
                context.Rejected();
                return;
            }

            try
            {
                string uid = context.OwinContext.Get<string>("SmartCard");

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, "Administrators"));

                identity.AddClaim(new Claim("MyClaim", "I don't know"));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {  "name", "John" },
                    { "surname", "Smith" },
                    { "age", "40" },
                    { "gender", "Male" }
                });

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);

            }
            catch (Exception)
            {
                // The ClaimsIdentity could not be created by the UserManager.
                context.Rejected();
                context.SetError("server_error");
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        //public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        //{
        //    return base.TokenEndpointResponse(context);
        //}

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var client = context.OwinContext.Get<ApplicationClient>("oauth:client");

            // var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            // enforce client binding of refresh token
            if (client.Id != currentClient)
            {
                context.Rejected();
                return;
            }

            // chance to change authentication ticket for refresh token requests
            var newId = new ClaimsIdentity(context.Ticket.Identity);
            newId.AddClaim(new Claim("newClaim", "refreshToken"));

            var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
            context.Validated(newTicket);
        }

    }
}
