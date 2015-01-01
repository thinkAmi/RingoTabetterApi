using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;
using CoreTweet;
using RingoTabetterApi.POCOs;
using RingoTabetterApi.Models;

namespace RingoTabetterTask.Helpers
{
    public class TwitterHelper
    {
        private const string FileName = @"twitter_settings.yaml";
        private const int NumberOfTweetPerApi = 200;

        private Tokens tokens;
        private string screenName;

        public TwitterHelper()
        {
            var apiKey = GetApiKey();
            screenName = apiKey.ScreenName;
            tokens = Tokens.Create(
                apiKey.ConsumerKey,
                apiKey.ConsumerSecret,
                apiKey.AccessKey,
                apiKey.AccessSecret
            );
        }


        /// <summary>
        /// 指定したフィルタでツイートを取得する
        /// 引数のフィルタがデフォルトの場合、全件取得になる
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TimelineResult GatherTweets(string filter = "")
        {
            var timeline = GatherTweetsByDescendingAfterLastSearch();
            if (!timeline.Any()) return new TimelineResult();

            var reg = new Regex(filter);
            return new TimelineResult()
            {
                SinceId = timeline.First().Id,
                Statuses = timeline.Where(t => reg.IsMatch(t.Text)).ToList()
            };
        }


        private IEnumerable<Status> GatherTweetsByDescendingAfterLastSearch()
        {
            // 今のところ、200件/回 * 10回  = 2,000件を上限に取得する
            var args = new Dictionary<string, object>() { { "screen_name", screenName }, { "count", NumberOfTweetPerApi } };

            // since_idが"0"の場合、「since_id parameter is invalid.」のCoreTweet.TwitterExceptionが発生するので、デフォルトは"1"にする
            var latestSearch = new LastSearches().Select();
            long sinceId = latestSearch.Any() ? latestSearch.Single().TweetId : 1;
            args.Add("since_id", sinceId);

            // 初回検索時のmax_idは、Twitterのmax_idの上限をセットしておく
            // long.MaxValueではツイートが取れなかったので、上限は long.Value - 1 の模様
            args.Add("max_id", long.MaxValue - 1);
            

            var result = new List<Status>();
            for (int i = 0; i < 10; i++)
            {
                var timeline = tokens.Statuses.UserTimeline(args);
                if (!timeline.Any()) break;

                var sortedTimeline = timeline.OrderByDescending(t => t.Id);
                result.AddRange(sortedTimeline);

                if (sortedTimeline.Count() < NumberOfTweetPerApi) break;

                args["max_id"] = sortedTimeline.Last().Id - 1;
            }

            return result;
        }


        private ApiKey GetApiKey()
        {
            if (File.Exists(FileName))
            {
                var input = new StreamReader(FileName, Encoding.UTF8);
                var deserializer = new YamlDotNet.Serialization.Deserializer();
                return deserializer.Deserialize<ApiKey>(input);
            }
            else
            {
                return new ApiKey()
                {
                    ScreenName = Environment.GetEnvironmentVariable("TWITTER_SCREEN_NAME"),
                    ConsumerKey = Environment.GetEnvironmentVariable("TWITTER_CONSUMER_KEY"),
                    ConsumerSecret = Environment.GetEnvironmentVariable("TWITTER_CONSUMER_SECRET"),
                    AccessKey = Environment.GetEnvironmentVariable("TWITTER_ACCESS_KEY"),
                    AccessSecret = Environment.GetEnvironmentVariable("TWITTER_ACCESS_SECRET")
                };
            }
        }


        public class TimelineResult
        {
            public long SinceId { get; set; }
            public IEnumerable<Status> Statuses { get; set; }
        }

        private class ApiKey
        {
            public string ScreenName { get; set; }
            public string ConsumerKey { get; set; }
            public string ConsumerSecret { get; set; }
            public string AccessKey { get; set; }
            public string AccessSecret { get; set; }
        }
    }



    
}
