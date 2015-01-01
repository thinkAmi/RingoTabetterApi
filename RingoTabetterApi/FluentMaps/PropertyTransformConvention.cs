using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RingoTabetterApi.FluentMaps
{
    public class PropertyTransformConvention : Dapper.FluentMap.Conventions.Convention
    {
        public PropertyTransformConvention()
        {
            Properties().Configure(c => c.Transform(
            s => Regex.Replace(s, "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4").ToLower()));
        }
    }
}
