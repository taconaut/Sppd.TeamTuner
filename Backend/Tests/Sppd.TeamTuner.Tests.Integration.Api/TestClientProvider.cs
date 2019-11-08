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
            return Program.CreateWebHostBuilder()
                          .UseUrls(SERVER_URL);
        }
    }
}