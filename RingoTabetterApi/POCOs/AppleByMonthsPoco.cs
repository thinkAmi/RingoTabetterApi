using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoTabetterApi.POCOs
{
    public class AppleByMonthsPoco
    {
        public string Name { get; set; }
        public int[] Quantities { get; set; }
        public string Color { get; set; }

        public AppleByMonthsPoco()
        {
            // Highchartsの仕様上、月別数量を配列として保持する必要があるため
            Quantities = new int[12];
        }
    }
}
