using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindBlazor.Database.Tests
{
    [TestClass]
    public class NorthwindDbContext_Tests
    {
        /// <summary>
        /// setup log to console if required
        /// </summary>
        public static readonly LoggerFactory ConsoleLogFactory= new LoggerFactory(new[] 
        {
            new ConsoleLoggerProvider((_, __) => true, true)
        });

        private NorthwindDbContext GetContext()
        {
            const string connectionString = "Server=[server];Database=Northwind;Uid=Blazor;Pwd=Blazor;";
            var builder = new DbContextOptionsBuilder<NorthwindDbContext>();
            builder.UseSqlServer(connectionString);
            //builder.UseLoggerFactory(ConsoleLogFactory); // enable to see EF log in console
            return new NorthwindDbContext(builder.Options);
        }

        [TestMethod]
        public async Task CheckConnectivity()
        {
            using (var db = GetContext())
            {
                var customers = await (from c in db.Customers
                                       orderby c.CompanyName
                                       select new Entities.CustomerModels.CompanyNameOnly()
                                       {
                                           CustomerId = c.CustomerId,
                                           CompanyName = c.CompanyName
                                       }).ToListAsync();

                foreach(var customer in customers)
                {
                    Console.WriteLine($"{customer.CustomerId}: {customer.CompanyName}");
                }
            }
        }
    }
}
