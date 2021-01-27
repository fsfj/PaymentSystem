using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Models
{
    public class Payments
    {
        [Key]
        public int ID { get; set; }

        public string UserCode { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }
    }

    public class PaymentListViewModel
    {
        public string Date { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }
    }

    public class AccountBalanceViewModel 
    { 
        public IEnumerable<PaymentListViewModel> PaymentList { get; set; }
        public decimal AccountBalance { get; set; }
    }
 }
