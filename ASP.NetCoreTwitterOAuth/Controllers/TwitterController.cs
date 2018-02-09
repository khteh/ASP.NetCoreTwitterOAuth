using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NetCoreTwitterOAuth.Data;
using ASP.NetCoreTwitterOAuth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASP.NetCoreTwitterOAuth.Controllers
{
    [Route("[controller]/[action]")]
    public class TwitterController : Controller
    {
        private readonly List<string> _screenNames = new List<string>() {"bbcnews", "bbcbreaking", "bbcworld", "bbcarabic", "alarabiya", "cnn", "cnnbrk", "cnnarabic",
            "reuters", "skynews", "skynewsarabia", "washingtonpost", "ap", "guardian", "nytimes", "time", "wsj", "vgnett", "dagbladet", "Aftenposten", "nrknyheter", "tv2nyhetene", "morgenbladet" };
        private readonly ITwitterService _twitterService;
        private readonly List<Tweet> _tweets = new List<Tweet>();
        public TwitterController(ITwitterService twitterService) => _twitterService = twitterService;
        [HttpGet]
        public ActionResult Index()
        {
            var sortedTweets = GetSortedTweets();
            // InvalidOperationException: The view 'Index' was not found. The following locations were searched: /Views/Home/Index.cshtml /Views/Shared/Index.cshtml
            return View("~/Pages/Twitter/Index.cshtml", sortedTweets.ToList());
        }
        [HttpGet]
        public IActionResult Partial()
        {
            var sortedTweets = GetSortedTweets();
            return PartialView("~/Pages/Partial/_Tweets.cshtml", sortedTweets);
        }
        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private IOrderedEnumerable<Tweet> GetSortedTweets()
        {
            foreach (var sn in _screenNames)
            {
                var tweetsJson = _twitterService.GetTweetsJson(sn);
                var tweet = JsonConvert.DeserializeObject<List<Tweet>>(tweetsJson).First();
                var cleanTweet = _twitterService.CleanText(tweet);
                _tweets.Add(cleanTweet);
            }
            var sortedTweets = _tweets.OrderByDescending(x => x.Id);
            return sortedTweets;
        }
    }
}