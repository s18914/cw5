using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Cw5.Handlers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger,            //wstrzyknięcie modułu do logowanie
            UrlEncoder encoder,               //odkoduje adres url
            ISystemClock clock)  : base(options, logger, encoder, clock)
        {

        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorisation"))
                return AuthenticateResult.Fail("Brak autoryzacji");

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorisation"]);
            var creBytes = Convert.FromBase64String(authHeader.Parameter);
            var cre = Encoding.UTF8.GetString(creBytes).Split(":");

            if(cre.Length!=2)
                return AuthenticateResult.Fail("Brak autoryzacji");

            //teraz sprawdzam czy taki user i hasło istnieją

            //pobrać role
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "employee")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);  //ticket

        }
    }
}
