using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoTabetterApi.POCOs
{
    public class ApplePoco
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long TweetId { get; set; }
        public DateTime TweetAt { get; set; }
        public string Tweet { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
