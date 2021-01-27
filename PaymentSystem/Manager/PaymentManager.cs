using PaymentSystem.Interface;
using PaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Manager
{
    public class PaymentManager : IPaymentManager
    {
        private readonly DatabaseContext _DatabaseContext;

        public PaymentManager(DatabaseContext DatabaseContext)
        {
            _DatabaseContext = DatabaseContext;
        }

        public IEnumerable<Payments> GetPaymentList(string userCode)
        {
            var paymentLists = (_DatabaseContext.Payments.ToList())
                .Where(i => i.UserCode == userCode).OrderByDescending(i => i.Date);

            return paymentLists;

        }
    }
}
