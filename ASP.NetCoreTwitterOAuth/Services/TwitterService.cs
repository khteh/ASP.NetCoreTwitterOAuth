using ASP.NetCoreTwitterOAuth.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi;
namespace ASP.NetCoreTwitterOAuth.Services
{
    public class TwitterService : ITwitterService
    {
        private string _consumerKey, _consumerSecret, _accessToken, _accessTokenSecret;
        private readonly ITwitterCredentials _credentials;
        private readonly List<ASP.NetCoreTwitterOAuth.Data.Tweet> _tweets;
        public TwitterService(string key, string secret, string token, string tokenSecret)
        {
            _tweets = new List<ASP.NetCoreTwitterOAuth.Data.Tweet>();
            _consumerKey = key;
            _consumerSecret = secret;
            _accessToken = token;
            _accessTokenSecret = tokenSecret;
            _credentials = Auth.SetUserCredentials(_consumerKey, _consumerSecret, _accessToken, _accessTokenSecret);
        }
        public ITwitterCredentials Credential() => _credentials;
        public async Task<string> GetTweetsJson(string screenName)
        {
            // Oauth application keys
            var oauth_token = _accessToken;
            var oauth_token_secret = _accessTokenSecret;
            var oauth_consumer_key = _consumerKey;
            var oauth_consumer_secret = _consumerSecret;

            // Oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            // Unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                            - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            // Message api details    
            var resource_url = "https://api.twitter.com/1.1/statuses/home_timeline.json";

            // Create oauth signature
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&screen_name={6}";

            var baseString = string.Format(baseFormat,
                oauth_consumer_key,
                oauth_nonce,
                oauth_signature_method,
                oauth_timestamp,
                oauth_token,
                oauth_version,
                Uri.EscapeDataString(screenName)
            );

            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
                oauth_signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));

            // Create the request header
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                                "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                                "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                                "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                Uri.EscapeDataString(oauth_nonce),
                Uri.EscapeDataString(oauth_signature_method),
                Uri.EscapeDataString(oauth_timestamp),
                Uri.EscapeDataString(oauth_consumer_key),
                Uri.EscapeDataString(oauth_token),
                Uri.EscapeDataString(oauth_signature),
                Uri.EscapeDataString(oauth_version)
            );
            // Make the request
            var postBody = "screen_name=" + Uri.EscapeDataString(screenName);
            resource_url += "?" + postBody;
            var request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers["Authorization"] = authHeader;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = await request.GetResponseAsync();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            return await reader.ReadToEndAsync();
        }
        public ASP.NetCoreTwitterOAuth.Data.Tweet CleanText(ASP.NetCoreTwitterOAuth.Data.Tweet tweet)
        {
            var cleanTweet = new ASP.NetCoreTwitterOAuth.Data.Tweet();
            cleanTweet = tweet;
            cleanTweet.Text = tweet.Text.Split(new[] { "https" }, StringSplitOptions.None)[0];
            return cleanTweet;
        }
        public async Task<IOrderedEnumerable<ASP.NetCoreTwitterOAuth.Data.Tweet>> GetTweetsAsync(string screenName)
        {
            string tweetsJson = await GetTweetsJson(screenName);
            List<ASP.NetCoreTwitterOAuth.Data.Tweet> tweets = JsonConvert.DeserializeObject<List<ASP.NetCoreTwitterOAuth.Data.Tweet>>(tweetsJson);
            foreach (ASP.NetCoreTwitterOAuth.Data.Tweet tweet in tweets)
                if (tweet != null)
                    _tweets.Add(CleanText(tweet));
            return _tweets.OrderByDescending(x => x.Id);
        }
    }
}