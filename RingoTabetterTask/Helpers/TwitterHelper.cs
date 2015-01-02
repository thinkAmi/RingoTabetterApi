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
        /// �w�肵���t�B���^�Ńc�C�[�g���擾����
        /// �����̃t�B���^���f�t�H���g�̏ꍇ�A�S���擾�ɂȂ�
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
            // ���̂Ƃ���A200��/�� * 10��  = 2,000��������Ɏ擾����
            var args = new Dictionary<string, object>() { { "screen_name", screenName }, { "count", NumberOfTweetPerApi } };

            // since_id��"0"�̏ꍇ�A�usince_id parameter is invalid.�v��CoreTweet.TwitterException����������̂ŁA�f�t�H���g��"1"�ɂ���
            var latestSearch = new LastSearches().Select();
            long sinceId = latestSearch.Any() ? latestSearch.Single().TweetId : 1;
            args.Add("since_id", sinceId);

            // ���񌟍�����max_id�́ATwitter��max_id�̏�����Z�b�g���Ă���
            // long.MaxValue�ł̓c�C�[�g�����Ȃ������̂ŁA����� long.Value - 1 �̖͗l
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
