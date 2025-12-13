using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using SCMS.Domain.TienPVK.Models;

namespace SCMS.BlazorWebApp.TienPVK.Services
{
    public static class ClaimService
    {
        public static ClaimsPrincipal GenerateClaimsPrincipal(SystemAccount account)
        {
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, account.UserAccountId.ToString()),
            new Claim(ClaimTypes.Name, account.UserName),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim("FullName", account.FullName),
            new Claim(ClaimTypes.Role, account.RoleId.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }
    }
}
