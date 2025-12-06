using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Service;


namespace ChargingManagement_PhanVuKhanhTien.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SystemUserService _userService;

        public LoginModel(SystemUserService userService) => _userService = userService;

        [BindProperty]
        public string UserName { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var userAccount = await _userService.LoginAsync(UserName, Password);

            if (userAccount != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userAccount.Username),
                    new Claim(ClaimTypes.Role, userAccount.UserRole.ToString()),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                //// After signing then redirect to default page
                return RedirectToPage("/ChargingSessions/Index");
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
