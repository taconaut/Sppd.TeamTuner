using System;
using System.IO;
using System.Net.Http;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.Tests.Integration.Api
{
    public class TestClientProvider
    {
        private const string SERVER_URL = "http://localhost:56789";

        public HttpClient Client { get; }

        public TestClientProvider()
        {
            var server = new TestServer(CreateWebHostBuilder());
            Client = server.CreateClient();
            Client.BaseAddress = new Uri(SERVER_URL);
        }

        private static IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              config.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), CoreConstants.Config.CONFIG_FOLDER));
                              config.AddJsonFile(CoreConstants.Config.APP_CONFIG_FILE_NAME, false, false);
                          })
                          .ConfigureLogging((hostingContext, logging) =>
                          {
                              logging.AddLog4Net(Path.Combine(CoreConstants.Config.CONFIG_FOLDER, CoreConstants.Config.LOG4NET_CONFIG_FILE_NAME));
                              logging.SetMinimumLevel(LogLevel.Trace);
                          })
                          .UseUrls(SERVER_URL)
                          .UseStartup<Startup>();
        }
    }
}