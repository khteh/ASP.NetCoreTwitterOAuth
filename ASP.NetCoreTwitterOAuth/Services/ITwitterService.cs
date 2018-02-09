using ASP.NetCoreTwitterOAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreTwitterOAuth.Services
{
    public interface ITwitterService
    {
        string GetTweetsJson(string screenName);
        Tweet CleanText(Tweet tweet);
    }
}