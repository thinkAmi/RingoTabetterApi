using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper.FluentMap;
using RingoTabetterApi.POCOs;

namespace RingoTabetterApi.FluentMaps
{
    public class LastSearchesMap : Dapper.FluentMap.Mapping.EntityMap<LastSearchPoco>
    {
        public LastSearchesMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.TweetId).ToColumn("tweet_id");
            Map(p => p.UpdatedAt).ToColumn("updated_at");
        }
    }
}
