using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Sambucks.Filters
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = request.Headers.Authorization;

            if (authHeader == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            if (authHeader.Scheme != "Basic")
            {
                return base.SendAsync(request, cancellationToken);
            }

            var encodedUserPass = authHeader.Parameter.Trim();
            var userPass = Encoding.ASCII.GetString(Convert.FromBase64String(encodedUserPass));
            var parts = userPass.Split(':');
            var username = parts[0];
            var identity = new GenericIdentity(username, "Basic");
            var roles = new[] { "customer" };
            if (username == "barista")
                roles[0] = "barista";
            var principal = new GenericPrincipal(identity, roles);
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}

