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
                Func<IEnumerable<AppleCountPoco>> func = () => new Highcharts().Total;
                return CreateJsonResponse(func);
            };


            Get["/month"] = _ =>
            {
                Func<IEnumerable<AppleCountPoco>> func = () => new Highcharts().TotalByMonth;
                return CreateJsonResponse(func);
            };
        }

        public Response CreateJsonResponse(Func<IEnumerable<AppleCountPoco>> func)
        {
            var appleCount = func();
            var response = Response.AsJson(appleCount);
            response.ContentType = "text/html; charset=utf8";
            return response;
        }
    }
}
