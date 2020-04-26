using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NorthwindBlazor.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindBlazor.Server.Services
{
    public class NorthwindDbService : INorthwindDbService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Get the Northwind DB context
        /// </summary>
        /// <param name="configuration"></param>
        public NorthwindDbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public NorthwindDbContext GetNorthwindDb()
        {
            var connectionString = _configuration.GetConnectionString("Northwind");
            var builder = new DbContextOptionsBuilder<NorthwindDbContext>();
            builder.UseSqlServer(connectionString);
            return new NorthwindDbContext(builder.Options);
        }
    }
}
