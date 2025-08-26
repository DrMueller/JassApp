using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JassApp.DataAccess.DbContexts.Factories.Implementation
{
    public class CommandInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<int> NonQueryExecuting(DbCommand c, CommandEventData e, InterceptionResult<int> r)
        {
            Console.WriteLine(c.CommandText);
            return r;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand c, CommandEventData e, InterceptionResult<DbDataReader> r)
        {
            Console.WriteLine(c.CommandText);
            return r;
        }

        public override InterceptionResult<object> ScalarExecuting(DbCommand c, CommandEventData e, InterceptionResult<object> r)
        {
            Console.WriteLine(c.CommandText);
            return r;
        }
    }
}