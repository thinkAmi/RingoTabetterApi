using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RingoTabetterApi.Models;
using RingoTabetterApi.POCOs;
using CoreTweet;
using System.Data;

namespace RingoTabetterTask.Helpers
{
    public class ModelHelper
    {
        /// <summary>
        /// トランザクションを使って、複数のテーブルへと登録する
        /// </summary>
        /// <param name="statuses"></param>
        /// <returns></returns>
        public static int RegisterWithTransaction(TwitterHelper.TimelineResult statuses)
        {
            if (statuses.SinceId == 0 && statuses.Statuses == null) return 0;

            int result = 0;
            using (var connection = RingoTabetterConnection.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        result = RegisterApples(connection, transaction, statuses.Statuses);
                        var count = RegisterLastSearch(connection, transaction, statuses.SinceId);

                        if (result != 0 || count != 0)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.ToString());
                        throw;
                    }
                }
            }

            return result;
        }


        private static int RegisterApples(IDbConnection connection, IDbTransaction transaction, IEnumerable<Status> statuses)
        {
            if (!statuses.Any()) return 0;

            var model = new Apple(connection, transaction);
            var apples = CategorizeTweets(statuses, model.Cultivar);

            var result = model.BulkInsert(apples);
            return result;
        }


        private static IEnumerable<ApplePoco> CategorizeTweets(IEnumerable<Status> statuses, Apple.CultivarYaml cultivar)
        {
            var results = new List<ApplePoco>();

            // 一つのツイートに複数の品種が含まれるかもしれないため、品種の方を回している
            foreach (var item in cultivar.Items)
            {
                var r = statuses
                    .Where(t => t.Text.Contains(item.Name))
                    .Select(t => new ApplePoco
                    {
                        Name = item.Name,
                        TweetId = t.Id,
                        TweetAt = t.CreatedAt.ToLocalTime().LocalDateTime,
                        Tweet = t.Text
                    });
                results.AddRange(r);
            }

            return results;
        }


        private static int RegisterLastSearch(IDbConnection connection, IDbTransaction transaction, long lastTweetId)
        {
            var model = new LastSearches(connection, transaction);
            var result = model.Merge(new LastSearchPoco() { TweetId = lastTweetId });
            return result;
        }
    }
}
