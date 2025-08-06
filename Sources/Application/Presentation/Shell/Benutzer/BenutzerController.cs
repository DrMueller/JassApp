using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JassApp.Presentation.Shell.Benutzer
{
    [Route("[controller]")]
    public class BenutzerController : Controller
    {
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> IdentityLogoutAsync(string returnUrl)
        {
            await HttpContext.SignOutAsync(new AuthenticationProperties
            {
                RedirectUri = returnUrl
            });

            return SignOut(OpenIdConnectDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
