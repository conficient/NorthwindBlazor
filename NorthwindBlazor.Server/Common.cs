using Microsoft.EntityFrameworkCore;
using NorthwindBlazor.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindBlazor.Server
{
    /// <summary>
    /// Common code for server
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Get a new Northwind DB context
        /// </summary>
        /// <returns></returns>
        public static NorthwindDbContext GetDb()
        {
            // TODO: move to configuration?
            const string connectionString = "Server=[server];Database=Northwind;Uid=Blazor;Pwd=Blazor;";
            var builder = new DbContextOptionsBuilder<NorthwindDbContext>();
            builder.UseSqlServer(connectionString);
            return new NorthwindDbContext(builder.Options);
        }

    }
}
