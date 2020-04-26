using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private NorthwindDbContext GetContext()
        {
            var config = Initialization.Configuration;
            string connectionString = config.GetConnectionString("northwind");
            var options = new DbContextOptionsBuilder<NorthwindDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            // create context
            var result = new NorthwindDbContext(options);
            // check DB exists, and if not, initialize it
            NorthwindBlazor.Database.NorthwindInitializer.Initialize(result);
            return result;
        }

        [TestMethod]
        public async Task CheckConnectivity()
        {
            using var db = GetContext();
            var customers = await
                (from c in db.Customers
                 orderby c.CompanyName
                 select new Entities.CustomerModels.CompanyNameOnly(c.CustomerId, c.CompanyName)
                 ).ToListAsync();

            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.CustomerId}: {customer.CompanyName}");
            }
        }
    }
}
