using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using RingoTabetterApi.POCOs;
using Dapper;

namespace RingoTabetterApi.Models
{
    public class LastSearches : ModelBase
    {
        public LastSearches() : base() { }
        public LastSearches(IDbConnection cn, IDbTransaction tr) : base(cn, tr) { }

        public IEnumerable<LastSearchPoco> Select()
        {
            Func<IEnumerable<LastSearchPoco>> func = () =>
            {
                var sql = "SELECT id, tweet_id, updated_at FROM last_searches";
                var r = transaction == null ?
                    connection.Query<LastSearchPoco>(sql) :
                    connection.Query<LastSearchPoco>(sql, transaction: transaction);
                return r;
            };

            var result = Query(func);
            return result;
        }

        public int Merge(LastSearchPoco entity)
        {
            var current = Select().FirstOrDefault();
            if (current != null && current.TweetId == entity.TweetId) return 0;

            var sql = current == null ?
                @"INSERT INTO last_searches(tweet_id) VALUES(@TweetId)" :
                @"UPDATE last_searches SET tweet_id = @TweetId";

            Func<int> func = () =>
            {
                var r = transaction == null ?
                    connection.Execute(sql, entity) :
                    connection.Execute(sql, entity, transaction: transaction);
                return r;
            };

            var result = Execute(func);
            return result;
        }
    }
}
