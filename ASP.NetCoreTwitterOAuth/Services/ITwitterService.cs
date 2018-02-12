using ASP.NetCoreTwitterOAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreTwitterOAuth.Services
{
    public interface ITwitterService
    {
        Task<string> GetTweetsJson(string screenName);
        Tweet CleanText(Tweet tweet);
        Task<IOrderedEnumerable<Tweet>> GetTweetsAsync();
    }
}