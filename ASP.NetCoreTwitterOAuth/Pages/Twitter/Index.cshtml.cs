using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NetCoreTwitterOAuth.Data;
using ASP.NetCoreTwitterOAuth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NetCoreTwitterOAuth.Pages.Twitter
{
    public class IndexModel : PageModel
    {
        private ITwitterService _service;
        public IOrderedEnumerable<Tweet> Tweets { get; set; }
        public IndexModel(ITwitterService service) => _service = service;
        public async Task OnGetAsync()
        {
            Tweets = await _service.GetTweetsAsync();
        }
    }
}