using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GigHub.IntegrationTest.Extensions
{
    public static class ControllerExtentions
    {
        public static void MockCurrentUser(this Controller controller, string userId, string username)
        {
            var identity = new GenericIdentity(username);
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", username)
                );
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId)
                );
            var principal = new GenericPrincipal(identity, null);

            controller.ControllerContext = Mock.Of<ControllerContext>(c =>
                c.HttpContext == Mock.Of<HttpContextBase>(h =>
                                    h.User == principal));
        }
    }
}
