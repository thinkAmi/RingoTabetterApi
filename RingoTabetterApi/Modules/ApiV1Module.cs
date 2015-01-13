using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy;
using RingoTabetterApi.Models;
using RingoTabetterApi.POCOs;

namespace RingoTabetterApi.Modules
{
    /// <summary>
    /// API v1用のモジュール
    /// </summary>
    public class ApiV1Module : NancyModule
    {
        public ApiV1Module() : base("/api/v1")
        {
            Get["/total"] = _ =>
            {
                var result = new Highcharts().Total;
                return Response.AsJson(result);
            };


            Get["/month"] = _ =>
            {
                var result = new Highcharts().TotalByMonth;
                return Response.AsJson(result);
            };
        }
    }
}
