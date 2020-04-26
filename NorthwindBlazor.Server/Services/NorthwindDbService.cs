using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        protected ILogger<NorthwindDbService> Log { get; }

        /// <summary>
        /// Get the Northwind DB context
        /// </summary>
        /// <param name="configuration"></param>
        public NorthwindDbService(IConfiguration configuration,
                                  ILogger<NorthwindDbService> logger)
        {
            _configuration = configuration;
            Log = logger;
        }

        public NorthwindDbContext GetNorthwindDb()
        {
            Log.LogDebug("GetNorthwindDb");
            var connectionString = _configuration.GetConnectionString("northwind");
            var builder = new DbContextOptionsBuilder<NorthwindDbContext>();
            builder.UseSqlServer(connectionString);
            var result = new NorthwindDbContext(builder.Options);
            if (hasNotBeenInitialized)
            {
                Log.LogDebug("GetNorthwindDb: initializing db");
                // check DB exists, and if not, initialize it
                NorthwindBlazor.Database.NorthwindInitializer.Initialize(result);
                hasNotBeenInitialized = false;
            }
            return result;

        }

        /// <summary>
        /// Flag to indicate we've checked DB initialization state
        /// </summary>
        private bool hasNotBeenInitialized = true;
    }
}
