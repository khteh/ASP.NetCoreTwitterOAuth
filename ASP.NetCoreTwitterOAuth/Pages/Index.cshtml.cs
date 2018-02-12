using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASP.NetCoreTwitterOAuth.Data;
using ASP.NetCoreTwitterOAuth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
namespace ASP.NetCoreTwitterOAuth.Pages.Twitter
{
    public class IndexModel : PageModel
    {
        private IAuthenticatedUser _user;
        private ITwitterService _service;
        private SignInManager<ApplicationUser> _signInManager;
        public IOrderedEnumerable<ASP.NetCoreTwitterOAuth.Data.Tweet> Tweets { get; set; }
        [BindProperty]
        public string TweetString { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
        public IndexModel(SignInManager<ApplicationUser> signInManager, ITwitterService service)
        {
            _signInManager = signInManager;
            _service = service;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (_user == null)
                    _user = Tweetinvi.User.GetAuthenticatedUser(_service.Credential());
                //Tweets = await _service.GetTweetsAsync();
                Tweets = await _service.GetTweetsAsync(_user.ScreenName);
                return Page();
            } else
                return RedirectToPage("/Account/Login");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (_signInManager.IsSignedIn(User))
            {
                ITwitterCredentials cre = _service.Credential();
                if (_user == null)
                    _user = Tweetinvi.User.GetAuthenticatedUser(cre);
                var fileBytes = GetByteArrayFromFile(Image);
                var publishedTweet = Auth.ExecuteOperationWithCredentials(cre, () =>
                {
                    var publishOptions = new PublishTweetOptionalParameters();
                    if (fileBytes != null)
                    {
                        publishOptions.MediaBinaries.Add(fileBytes);
                    }
                    return Tweetinvi.Tweet.PublishTweet(TweetString, publishOptions);
                });
                bool success = publishedTweet != null;
                var routeValueParameters = new Dictionary<string, object>();
                routeValueParameters.Add("id", publishedTweet == null ? (Nullable<long>)null : publishedTweet.Id);
                routeValueParameters.Add("actionPerformed", "Publish");
                routeValueParameters.Add("success", success);
                if (success)
                    return await OnGetAsync();
                else
                    return RedirectToPage();
            } else
                return RedirectToPage("/Account/Login");
        }
        #region Private Functions
        private byte[] GetByteArrayFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;
            var memoryStream = new MemoryStream();
            file.OpenReadStream().CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        #endregion
    }
}