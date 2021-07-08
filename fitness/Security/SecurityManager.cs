using fitness.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;


namespace fitness.Security
{
    public class SecurityManager
    {
        public async void SignIn(HttpContext httpContext, Account account, string schema)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(getUserClaims(account),schema);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await httpContext.SignInAsync(schema, claimsPrincipal);
        }
        public async void SignOut(HttpContext httpContext, string schema)
        {
            await httpContext.SignOutAsync(schema);
        }

        public IEnumerable<Claim> getUserClaims(Account account)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, account.username));
            //foreach(var roleAccount in account.RoleAccounts)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, roleAccount.Role.Name));
            //}

            return claims;
        }
    }
}
