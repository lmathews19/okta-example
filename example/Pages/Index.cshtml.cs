using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Okta.AspNetCore;

namespace example.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        //public void OnGet()
        //{

        //}
        public async Task<IActionResult> OnGet()
        {
            //if (!HttpContext.User.Identity.IsAuthenticated)
            //{
            //    return Challenge(OktaDefaults.MvcAuthenticationScheme);
            //}
            return Page();
        }

    }
}