using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindBlazor.Database
{
    [TestClass]
    public static class Initialization
    {

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _)
        {
            Console.WriteLine("AssemblyInitialize: load configuration");
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup");
        }

        public static IConfiguration Configuration { get; private set; }
    }
}
