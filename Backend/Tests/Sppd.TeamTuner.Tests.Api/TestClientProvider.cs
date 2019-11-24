using System;
using System.Net.Http;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Sppd.TeamTuner.Tests.Api
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