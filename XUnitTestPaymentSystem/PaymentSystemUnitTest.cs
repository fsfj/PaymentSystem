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
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Manager;

namespace XUnitTestPaymentSystem
{
    public class PaymentSystemUnitTest
    {
        private readonly DatabaseContext _databaseContext;

        public PaymentSystemUnitTest()
        {
            DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "Payments")
            .Options;

            var context = new DatabaseContext(options);

            if (!context.Users.Any())
            {
                // the data will be seeded here.

                context.Users.AddRange(
                new Users
                {
                    ID = 1,
                    UserCode = "00001",
                    LastName = "LastName1",
                    FirstName = "FirstName1",
                    MiddleName = "MiddleName1",
                    Username = "usertest1",
                    AccountNumber = "1234567890"
                },
                new Users
                {
                    ID = 2,
                    UserCode = "00002",
                    LastName = "LastName2",
                    FirstName = "FirstName2",
                    MiddleName = "MiddleName2",
                    Username = "usertest2",
                    AccountNumber = "0987654321"
                });
            }

            if (!context.Payments.Any())
            {
                // the data will be seeded here.

                context.Payments.AddRange(
                new Payments
                {
                    ID = 1,
                    UserCode = "00001",
                    Date = Convert.ToDateTime("2021-01-22 03:00"),
                    Amount = 1002.33,
                    Status = "Open",
                    Reason = ""
                },
                new Payments
                {
                    ID = 2,
                    UserCode = "00001",
                    Date = Convert.ToDateTime("2021-01-23 04:00"),
                    Amount = 1202.36,
                    Status = "Open",
                    Reason = ""
                },
                new Payments
                {
                    ID = 3,
                    UserCode = "00001",
                    Date = Convert.ToDateTime("2021-01-24 05:00"),
                    Amount = -123.36,
                    Status = "Closed",
                    Reason = "Test Payment"
                },
                new Payments
                {
                    ID = 4,
                    UserCode = "00002",
                    Date = Convert.ToDateTime("2021-01-24 23:00"),
                    Amount = -1030.36,
                    Status = "Closed",
                    Reason = "Test Payment 2"
                },
                new Payments
                {
                    ID = 5,
                    UserCode = "00002",
                    Date = Convert.ToDateTime("2021-01-26 18:00"),
                    Amount = 1430.36,
                    Status = "Open",
                    Reason = ""
                });
            }

            if (!context.UserCredentials.Any())
            {
                // the data will be seeded here.

                context.UserCredentials.AddRange(
                new UserCredentials
                {
                    ID = 1,
                    Username = "usertest1",
                    Password = "CmTXmMMVWag/NXP3BhKUk9FlSyrbPIsdyyyjDFNuW2Y="
                },
                new UserCredentials
                {
                    ID = 2,
                    Username = "usertest2",
                    Password = "CmTXmMMVWag/NXP3BhKUk/UOWcqxS/fRJvCA5TFIrPY="
                });
            }

            context.SaveChanges();

            _databaseContext = context;
        }

        [Fact]
        public async Task TestGetPaymentsApi()
        {
            var authenticationManager = new PaymentSystem.Manager.AuthenticationManager(_databaseContext);

            using (var client = new TestClientProvider().Client)
            {
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", authenticationManager.Authenticate("usertest1", "password1")); // this test should be unauthorize since paswword is incorrect

                var response = await client
                    .GetAsync("/api/payments/getpayments");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            _databaseContext.Dispose();
        }

        [Fact]
        public async Task TestGetPaymentsApiInvalidCredentials()
        {
            var authenticationManager = new PaymentSystem.Manager.AuthenticationManager(_databaseContext);

            using (var client = new TestClientProvider().Client)
            {
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", authenticationManager.Authenticate("usertest1", "password2")); // this test should be unauthorize since paswword is incorrect

                var response = await client
                    .GetAsync("/api/payments/getpayments");

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }

            _databaseContext.Dispose();
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

                _databaseContext.Dispose();

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public async Task TestGetUser()
        {
            var userManager = new UserManager(_databaseContext);
            Users expectedResult = await Task.Run(() => _databaseContext.Users.FirstOrDefault(i => i.Username == "usertest1"));
            Users actualResult = await Task.Run(() => userManager.GetUser("usertest1"));

            Assert.Equal(expected : expectedResult.ID, actual : actualResult.ID);
        }

        [Fact]
        public async Task TestGetPaymentList()
        {
            var paymentManager = new PaymentManager(_databaseContext);
            var userManager = new UserManager(_databaseContext);

            Users user = userManager.GetUser("usertest1");

            IEnumerable<Payments> actualResult = await Task.Run(() => paymentManager.GetPaymentList(user.UserCode));
            IEnumerable<Payments> expectedResult = await Task.Run(() => (_databaseContext.Payments.ToList()).Where(i => i.UserCode == user.UserCode).OrderByDescending(i => i.Date));

            Assert.Equal(expected: expectedResult, actual: actualResult);
        }

    }
}
