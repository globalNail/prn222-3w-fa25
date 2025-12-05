using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SCMS.Service.TienPVK.Implements;


namespace SCMS.RazorWebApp.TienPVK.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SystemAccountService _userAccountService;

        public LoginModel() => _userAccountService ??= new SystemAccountService();

        [BindProperty]
        public string UserName { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var userAccount = await _userAccountService.LoginAsync(UserName, Password);

            if (userAccount != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userAccount.UserName),
                    new Claim(ClaimTypes.Role, userAccount.RoleId.ToString()),
                    new Claim("FullName", userAccount.FullName),
                    new Claim("UserId", userAccount.UserAccountId.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                //// After signing then redirect to default page
                return RedirectToPage("/ClubsTienPvks/Index");
                //return RedirectToPage("/Index");
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                TempData["Message"] = "Login fail, please check your account";
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Page();
        }
    }
}
