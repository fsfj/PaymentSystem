using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using PaymentSystem;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace XUnitTestPaymentSystem
{
    public class TestClientProvider
    {
        private TestServer server;
        public HttpClient Client { get; private set; }

        public TestClientProvider()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            Client = server.CreateClient();
        }

        public void Dispose() 
        {
            server?.Dispose();
            Client?.Dispose();
        }
    }
}
