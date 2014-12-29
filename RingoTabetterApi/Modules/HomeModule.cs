using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy;

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
        }
    }
}
