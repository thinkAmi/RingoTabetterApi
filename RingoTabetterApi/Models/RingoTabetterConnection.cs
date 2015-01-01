using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Npgsql;

namespace RingoTabetterApi.Models
{
    public class RingoTabetterConnection
    {
        public static IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(GetConnectionString());
        }

        private static string GetConnectionString()
        {
            if (Utility.IsProduction())
            {
                var uri = new Uri(Environment.GetEnvironmentVariable("DATABASE_URL"));
                var userInfo = uri.UserInfo.Split(':');


                return string.Format("Server={0}; Port={1}; Database={2}; User Id={3}; Password={4}; SSL=true;SslMode=Require;",
                    uri.Host,
                    uri.Port,
                    uri.Segments.Last(),
                    userInfo[0],
                    userInfo[1]
                    );
            }
            else
            {
                return "Server=127.0.0.1; Port=5432; Database=ringotabetter; User Id=postgres; Password=postgres;";
            }
        }
    }
}
