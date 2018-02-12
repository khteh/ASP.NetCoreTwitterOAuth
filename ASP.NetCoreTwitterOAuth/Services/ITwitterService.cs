using ASP.NetCoreTwitterOAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace ASP.NetCoreTwitterOAuth.Services
{
    public interface ITwitterService
    {
        ITwitterCredentials Credential();
        Task<string> GetTweetsJson(string screenName);
        Tweet CleanText(Tweet tweet);
        Task<IOrderedEnumerable<Tweet>> GetTweetsAsync();
        Task<IOrderedEnumerable<ASP.NetCoreTwitterOAuth.Data.Tweet>> GetTweetsAsync(string screenName);
    }
}