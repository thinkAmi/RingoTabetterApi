using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoTabetterApi.POCOs
{
    public class LastSearchPoco
    {
        public int Id { get; set; }
        public long TweetId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
