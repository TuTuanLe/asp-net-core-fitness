using fitness.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace fitness.Security
{
    public class SecurityManager
    {
        public async void SignIn(HttpContext httpContext, Account account)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(getUserClaims(account),
                CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }
        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        public IEnumerable<Claim> getUserClaims(Account account)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, account.username));
            account.RoleAccounts.ToList().ForEach(ra =>
                claims.Add(new Claim(ClaimTypes.Role, ra.Role.Name))
             );
            return claims;
        }
    }
}
