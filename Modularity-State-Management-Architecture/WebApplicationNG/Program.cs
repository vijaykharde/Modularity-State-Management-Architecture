using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using ConfigurationSection = Microsoft.Extensions.Configuration.ConfigurationSection;

namespace WebApplicationNG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new ShellApplication().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
