using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;
using Dapper;
using System.Data;

namespace RingoTabetterApi.Models
{
    public class ModelBase
    {
        protected IDbConnection connection;
        protected IDbTransaction transaction;

        public ModelBase()
        {
            connection = RingoTabetterConnection.CreateConnection();
        }

        public ModelBase(IDbConnection cn, IDbTransaction tr)
        {
            connection = cn;
            transaction = tr;
        }


        protected int Execute(Func<int>func)
        {
            int result = 0;

            if (connection.State == ConnectionState.Open)
            {
                result = func();
                return result;
            }
            
            using (connection)
            {
                try
                {
                    connection.Open();

                    result = func();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }


        protected IEnumerable<T> Query<T>(Func<IEnumerable<T>> func)
        {
            IEnumerable<T> result;

            if (connection.State == ConnectionState.Open)
            {
                result = func();
                return result;
            }

            using (connection)
            {
                try
                {
                    connection.Open();
                    result = func();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
    }
}
