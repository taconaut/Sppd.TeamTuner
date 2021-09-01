using System;
using System.IO;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Workaround to avoid having the current directory C:\Program Files\IIS Express when debugging from Visual Studio
            // Otherwise at least configurations and log files will be read/created there
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args = null)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .ConfigureAppConfiguration((_, config) =>
                          {
                              config.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), CoreConstants.Config.CONFIG_FOLDER));
                              config.AddJsonFile(CoreConstants.Config.APP_CONFIG_FILE_NAME, false, false);
                          })
                          .ConfigureLogging((_, logging) =>
                          {
                              logging.AddLog4Net(Path.Combine(CoreConstants.Config.CONFIG_FOLDER, CoreConstants.Config.LOG4NET_CONFIG_FILE_NAME));
                          })
                          .UseStartup<Startup>();
        }
    }
}