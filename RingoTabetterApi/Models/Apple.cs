using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using RingoTabetterApi.POCOs;
using Dapper;
using System.Data;

namespace RingoTabetterApi.Models
{
    public class Apple : ModelBase
    {
        private const string FileName = @"apples.yaml";

        private CultivarYaml _cultivar;
        public CultivarYaml Cultivar
        {
            get
            {
                if (_cultivar == null)
                {
                    _cultivar = LoadCultivar();
                }

                return _cultivar;
            }
        }


        public Apple() : base() { }
        public Apple(IDbConnection cn, IDbTransaction tr) : base(cn, tr) { }


        public IEnumerable<TotalApplePoco> AddUp()
        {
            Func<IEnumerable<TotalApplePoco>> func = () =>
            {
                var sql = @"SELECT COUNT(name) as quantity, name FROM apples GROUP BY name ORDER BY quantity DESC";

                var result = transaction == null ?
                    connection.Query<TotalApplePoco>(sql) :
                    connection.Query<TotalApplePoco>(sql, transaction: transaction);
                return result;
            };

            var apples = Query(func);
            return apples;
        }

        public IEnumerable<TotalApplePoco> AddUpByMonths()
        {
            Func<IEnumerable<TotalApplePoco>> func = () =>
            {
                var sql = @"SELECT COUNT(name) as quantity, name, date_part('month', tweet_at) as month FROM apples ";
                sql += "GROUP BY name, month ORDER BY name, month";

                var result = transaction == null ?
                    connection.Query<TotalApplePoco>(sql) :
                    connection.Query<TotalApplePoco>(sql, transaction: transaction);
                return result;
            };

            var apples = Query(func);
            return apples;
        }


        public int BulkInsert(IEnumerable<ApplePoco> apples)
        {
            Func<int> func = () =>
            {
                var sql = @"INSERT INTO apples(name, tweet_id, tweet_at, tweet) ";
                sql += @"VALUES(@Name, @TweetId, @TweetAt, @Tweet)";

                var result = transaction == null ?
                    connection.Execute(sql, apples) :
                    connection.Execute(sql, apples, transaction: transaction);
                return result;
            };

            var r = Execute(func);
            return r;
        }


        public CultivarYaml LoadCultivar()
        {
            using (var input = new StreamReader(FileName, Encoding.UTF8))
            {
                var deserializer = new YamlDotNet.Serialization.Deserializer();
                return deserializer.Deserialize<CultivarYaml>(input);
            }
        }


        public class CultivarYaml
        {
            public List<Item> Items { get; set; }
        }

        public class Item
        {
            public string Name { get; set; }
            public string Color { get; set; }
        }
    }


}
