using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy;
using RingoTabetterApi.Models;

namespace RingoTabetterApi.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => "Hello World";

            Get["/json"] = _ =>
            {
                var ringo = new[]
                {
                    new { Id = 1, ItemName = "フジ"},
                    new { Id = 2, ItemName = "秋映"}
                };

                return Response.AsJson(ringo);
            };

            Get["/total"] = _ =>
            {
                var highcharts = new Highcharts();
                var result = highcharts.Total;

                return Response.AsJson(result);
            };

            Get["/month"] = _ =>
            {
                var highcharts = new Highcharts();
                var result = highcharts.TotalByMonth;
                return Response.AsJson(result);
            };
        }
    }
}
