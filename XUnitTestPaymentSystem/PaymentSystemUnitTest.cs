using System;
using Xunit;
using PaymentSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using FluentAssertions;
using System.Net.Http.Headers;

namespace XUnitTestPaymentSystem
{
    public class PaymentSystemUnitTest
    {
        [Fact]
        public async Task TestGetPaymentsApi()
        {
            var authenticationManager = new PaymentSystem.Models.AuthenticationManager("this is my secret key");

            using (var client = new TestClientProvider().Client)
            {
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", authenticationManager.Authenticate("usertest1", "password1"));

                var response = await client
                    .GetAsync("/api/payments/getpayments");
                
                response.EnsureSuccessStatusCode();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task TestGetPaymentsApiInvalidCredentials()
        {
            var authenticationManager = new PaymentSystem.Models.AuthenticationManager("this is my secret key");

            using (var client = new TestClientProvider().Client)
            {
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", authenticationManager.Authenticate("usertest1", "password2")); // this test should be unauthorize since paswword is incorrect

                var response = await client
                    .GetAsync("/api/payments/getpayments");

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public async Task TestAuthenticateApi()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/api/account/authenticate"
                        , new StringContent(
                        JsonConvert.SerializeObject(new UserCredentials()
                        {
                            Username = "usertest1",
                            Password = "password1"
                        }),
                    Encoding.UTF8,
                    "application/json"));

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestAuthenticateApiInvalidCredentials()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/api/account/authenticate"
                        , new StringContent(
                        JsonConvert.SerializeObject(new UserCredentials()
                        {
                            Username = "usertest1",
                            Password = "password2" // this test should be unauthorize since paswword is incorrect
                        }),
                    Encoding.UTF8,
                    "application/json"));

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public void GetUserResultTesting()
        {
            var userManager = new UserManager();

            Users expectedResult = new Users { ID = 1, Username = "usertest1", UserCode = "USER0001", AccountNumber = "000001", LastName = "LName1", FirstName = "FName1", MiddleName = "MName1" };
            Users actualResult = userManager.GetUser("usertest1");

            Assert.Equal(expectedResult.ID, actualResult.ID);
        }
    }
}
