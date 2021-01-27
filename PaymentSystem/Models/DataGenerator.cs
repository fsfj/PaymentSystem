using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Models
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DatabaseContext(
                serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                // Look for any board games.
                if (context.Users.Any()) {
                    return;   // Data was already seeded
                }

                if (context.Payments.Any()) {
                    return;   // Data was already seeded
                }

                if (context.UserCredentials.Any())
                {
                    return;   // Data was already seeded
                }

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
                        Amount =  -1030.36,
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

                context.SaveChanges();
            }
        }
    }
}
