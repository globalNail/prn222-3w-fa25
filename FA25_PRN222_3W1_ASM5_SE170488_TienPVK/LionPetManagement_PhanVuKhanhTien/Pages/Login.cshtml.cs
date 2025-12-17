using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Service;


namespace LionPetManagement_PhanVuKhanhTien.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly LionAccountService _userAccountService;

        public LoginModel() => _userAccountService ??= new LionAccountService();

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
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                //// After signing then redirect to Index
                return RedirectToPage("/LionProfiles/Index");
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                TempData["Message"] = "Invalid Email or Password!";
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Page();
        }
    }
}
