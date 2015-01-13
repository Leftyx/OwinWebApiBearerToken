namespace OwinWebApiBearerToken.api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Web.Http;

    [RoutePrefix("api")]
    public class CustomersController : ApiController
    {
        [Route("customer/{id:int}")]
        [Authorize(Roles="Administrators")]
        public Models.Customer Get(int id)
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            var myClaim = principal.Claims.Where(f => f.Type == "MyClaim").SingleOrDefault();

            return new Models.Customer()
            {
                ID = 1,             
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
