using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Okta.AspNetCore;

namespace example.Pages.Account
{
    public class ProfileModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            //if (!HttpContext.User.Identity.IsAuthenticated)
            //{
            //    return Challenge(OktaDefaults.MvcAuthenticationScheme);
            //}
            return Page();
        }

        public IEnumerable<System.Security.Claims.Claim> Claims
        {
            get => User?.Claims;
        }
    }
}
