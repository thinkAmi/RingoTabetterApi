using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RingoTabetterApi.POCOs;

namespace RingoTabetterApi.Models
{
    public class Highcharts
    {
        private IEnumerable<AppleCountPoco> _total;
        public IEnumerable<AppleCountPoco> Total
        {
            get
            {
                if (_total == null)
                {
                    var apple = new Apple();
                    var appleCount = apple.AddUp();

                    _total = appleCount
                        .Join(apple.Cultivar.Items, a => a.Name, c => c.Name, (a, c) => new AppleCountPoco
                        {
                            Name = a.Name,
                            Quantity = a.Quantity,
                            Color = c.Color
                        });
                }
                return _total;
            }
        }


        private IEnumerable<AppleCountPoco> _totalByMonth;
        public IEnumerable<AppleCountPoco> TotalByMonth
        {
            get
            {
                if (_totalByMonth == null)
                {
                    var apple = new Apple();
                    var appleCount = apple.AddUpPerMonth();

                    _totalByMonth = appleCount
                        .Join(apple.Cultivar.Items, a => a.Name, c => c.Name, (a, c) => new AppleCountPoco
                        {
                            Name = a.Name,
                            Month = a.Month,
                            Quantity = a.Quantity,
                            Color = c.Color
                        });
                }
                return _totalByMonth;
            }
        }
    }
}
