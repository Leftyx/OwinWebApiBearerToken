using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace OwinWebApiBearerToken.api
{
    [RoutePrefix("api")]
    public class CustomersController : ApiController
    {
        [Route("customer/{id:int}")]
        [Authorize(Roles="Administrators")]
        public Models.Customer Get(int id)
        {
            var principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            var myClaim = principal.Claims.SingleOrDefault(f => f.Type == "MyClaim");

            return new Models.Customer
            {
                Id = id,
                LastName = "Smith",
                FirstName = "Mary",
                HouseNumber = "333",
                Street = "Main Street NE",
                City = "Redmond",
                State = "WA",
                ZipCode = "98053"
            };
        }
    }
}
