using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;
using RingoTabetterApi.POCOs;
using RingoTabetterApi.Models;
using Dapper;
using Dapper.FluentMap;
using RingoTabetterApi.FluentMaps;
using RingoTabetterTask.Helpers;

using System.Text.RegularExpressions;

namespace RingoTabetterTask
{
    class Program
    {
        static void Main(string[] args)
        {
            // DBとModelのマッピング
            FluentMapper.Intialize(config =>
            {
                var type = typeof(Apple);
                config.AddConvention<PropertyTransformConvention>().ForEntitiesInAssembly(type.Assembly, type.Namespace);
                config.AddMap(new LastSearchesMap());
            });

            var twitterHelper = new TwitterHelper();
            var tweets = twitterHelper.GatherTweets();
            Console.WriteLine("Get Tweet Count: {0}", tweets.Statuses == null ? 0 : tweets.Statuses.Count());

            var count = ModelHelper.RegisterWithTransaction(tweets);
            Console.WriteLine("Running: {0}, Registered Count: {1}", DateTime.Now, count);
        }
    }
}
