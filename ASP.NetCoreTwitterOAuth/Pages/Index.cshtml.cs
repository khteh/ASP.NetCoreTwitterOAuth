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
        private SignInManager<ApplicationUser> _signInManager;
        public IOrderedEnumerable<ASP.NetCoreTwitterOAuth.Data.Tweet> Tweets { get; set; }
        [BindProperty]
        public string TweetString { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
        public string ScreenName { get; set; }
        public List<string> Friends { get; set; }
        //public IndexModel(SignInManager<ApplicationUser> signInManager, ITwitterService service)
        public IndexModel(SignInManager<ApplicationUser> signInManager) => _signInManager = signInManager;
        public async Task<IActionResult> OnGetAsync()
        {
            if (_signInManager.IsSignedIn(User))
            {
                HandleAuthenticatedUser();
                //Tweets = await _service.GetTweetsAsync(_user.ScreenName);
                return Page();
            } else
                return RedirectToPage("/Account/Login");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (_signInManager.IsSignedIn(User))
            {
                HandleAuthenticatedUser();
                var publishedTweet = Auth.ExecuteOperationWithCredentials(_user.Credentials, PublishTweet);
                bool success = publishedTweet != null;
                var routeValueParameters = new Dictionary<string, object>();
                routeValueParameters.Add("id", publishedTweet == null ? (Nullable<long>)null : publishedTweet.Id);
                routeValueParameters.Add("actionPerformed", "Publish");
                routeValueParameters.Add("success", success);
                return RedirectToPage();
            } else
                return RedirectToPage("/Account/Login");
        }
        #region Private Functions
        private ITweet PublishTweet()
        {
            var fileBytes = GetByteArrayFromFile(Image);
            var publishOptions = new PublishTweetOptionalParameters();
            if (fileBytes != null)
                publishOptions.MediaBinaries.Add(fileBytes);
            return Tweetinvi.Tweet.PublishTweet(TweetString, publishOptions);
        }
        private byte[] GetByteArrayFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;
            var memoryStream = new MemoryStream();
            file.OpenReadStream().CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        private void HandleAuthenticatedUser()
        {
            if (_user == null)
            {
                _user = Tweetinvi.User.GetAuthenticatedUser();
                ScreenName = _user.ScreenName;
                Friends = _user.GetFriends().Select(i => i.ScreenName).ToList();
            }
        }
        #endregion
    }
}