using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoTabetterApi
{
    public class Utility
    {
        public static bool IsProduction()
        {
            return System.Environment.GetEnvironmentVariable("DATABASE_URL") != null;
        }
    }
}
