using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NetCoreTwitterOAuth.Data;
using ASP.NetCoreTwitterOAuth.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NetCoreTwitterOAuth.Pages.Twitter
{
    public class IndexModel : PageModel
    {
        private ITwitterService _service;
        private SignInManager<ApplicationUser> _signInManager;
        public IOrderedEnumerable<Tweet> Tweets { get; set; }
        public IndexModel(SignInManager<ApplicationUser> signInManager, ITwitterService service)
        {
            _signInManager = signInManager;
            _service = service;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (_signInManager.IsSignedIn(User))
            {
                Tweets = await _service.GetTweetsAsync();
                return Page();
            } else
                return RedirectToPage("/Account/Login");
        }
    }
}