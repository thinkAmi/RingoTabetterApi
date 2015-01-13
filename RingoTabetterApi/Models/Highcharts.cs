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
        private IEnumerable<TotalApplePoco> _total;
        public IEnumerable<TotalApplePoco> Total
        {
            get
            {
                if (_total == null)
                {
                    var apple = new Apple();
                    var appleCount = apple.AddUp();

                    _total = appleCount
                        .Join(apple.Cultivar.Items, a => a.Name, c => c.Name, (a, c) => new TotalApplePoco
                        {
                            Name = a.Name,
                            Quantity = a.Quantity,
                            Color = c.Color
                        });
                }
                return _total;
            }
        }


        private IEnumerable<AppleByMonthsPoco> _totalByMonth;
        public IEnumerable<AppleByMonthsPoco> TotalByMonth
        {
            get
            {
                if (_totalByMonth == null)
                {
                    var apple = new Apple();
                    var sum = apple.AddUpByMonths();

                    // Highchartsで使うため、月別数量を縦持ちしているDBデータを、配列Quantitiesで横持ちにする
                    var result = new Dictionary<string, AppleByMonthsPoco>();
                    foreach (var s in sum)
                    {
                        if (result.ContainsKey(s.Name))
                        {
                            result[s.Name].Quantities[s.Month - 1] = s.Quantity;
                        }
                        else
                        {
                            var color = apple.Cultivar.Items.Where(c => c.Name == s.Name).FirstOrDefault().Color ?? "Black";
                            var row = new AppleByMonthsPoco();
                            row.Name = s.Name;
                            row.Quantities[s.Month - 1] = s.Quantity;
                            row.Color = color;
                            result.Add(s.Name, row);
                        }
                    }
                    _totalByMonth = result.Values;
                }
                return _totalByMonth;
            }
        }
    }
}
