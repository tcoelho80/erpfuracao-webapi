using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace ERP.Furacao.Infrastructure.Data.Helpers
{
    public static class ConnectionHelper<T>
    {
        public static IDbConnection DbConnection { get; set; }

        public static async Task<T> ExecuteFirst(Func<Task<T>> code)
        {
            return (T)await ExecuteConnectionAsync(async () =>
                 await code()
              );
        }

        public static async Task<IEnumerable<T>> ExecuteList(Func<Task<IEnumerable<T>>> code)
        {
            return (IEnumerable<T>)await ExecuteConnectionAsync(async () =>
                await code()
             );
        }

        public static T ExecuteFirst(Func<T> code)
        {
            return (T)ExecuteConnection(() =>
                code()
              );
        }

        public static IEnumerable<T> ExecuteList(Func<IEnumerable<T>> code)
        {
            return (IEnumerable<T>)ExecuteConnection(() =>
               code()
             );
        }

        public static void Execute(Func<object> code)
        {
            ExecuteConnection(() => code());
        }

        private static object ExecuteConnection(Func<object> code)
        {
            DbConnection.Open();

            try
            {
                return code();
            }
            finally
            {
                DbConnection.Close();
            }
        }

        private static async Task<object> ExecuteConnectionAsync(Func<Task<object>> code)
        {
            DbConnection.Open();

            try
            {
                return await code();
            }
            finally
            {
                DbConnection.Close();
            }
        }

    }

    public static class ConnectionHelper
    {
        public static IConfiguration ConnectionConfiguration
        {
            get
            {
                IConfigurationRoot Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                return Configuration;
            }
        }
    }
}
