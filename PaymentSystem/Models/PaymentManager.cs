using PaymentSystem.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Models
{
    public class PaymentManager : IPaymentManager
    {
        private readonly DatabaseContext _DatabaseContext;

        public PaymentManager(DatabaseContext DatabaseContext)
        {
            _DatabaseContext = DatabaseContext;
        }

        //private readonly IList<Payments> payments = new List<Payments>
        //{
        //    new Payments { ID = 1, UserCode = "USER0001", Date = Convert.ToDateTime("2021-01-15 13:30"), Amount = 1000, Status = "Closed", Reason = "" },
        //    new Payments { ID = 2, UserCode = "USER0001", Date = Convert.ToDateTime("2021-01-16 14:30"), Amount = 400, Status = "Closed", Reason = "Test Reason" },
        //    new Payments { ID = 3, UserCode = "USER0001", Date = Convert.ToDateTime("2021-01-17 12:30"), Amount = -300, Status = "Closed", Reason = "" },
        //    new Payments { ID = 4, UserCode = "USER0002", Date = Convert.ToDateTime("2021-01-15 03:30"), Amount =  5300, Status = "Closed", Reason = "" },
        //    new Payments { ID = 5, UserCode = "USER0002", Date = Convert.ToDateTime("2021-01-17 21:30"), Amount = -600, Status = "Closed", Reason = "" },
        //};

        public IEnumerable<PaymentListViewModel> GetPaymentList(string userCode)
        {
            var paymentLists = (_DatabaseContext.Payments.ToList())
                .Where(i => i.UserCode == userCode)
                .Select(i => new PaymentListViewModel {
                    Date = i.Date.ToString("yyyy-MM-dd hh:mm tt"),
                    Amount = i.Amount,
                    Status = i.Status,
                    Reason = i.Reason
                }).OrderByDescending(i => i.Date);

            //var paymentLists = 
            //    payments
            //    .Where(i => i.UserCode == userCode)
            //    .Select(i => new PaymentListViewModel
            //    {
            //        Date = i.Date.ToString("yyyy-MM-dd hh:mm tt"), // * this is to properly display date in the json output *
            //        Amount = i.Amount,
            //        Status = i.Status,
            //        Reason = i.Reason
            //    }).OrderByDescending(i => i.Date);

            return paymentLists;

        }
    }
}
