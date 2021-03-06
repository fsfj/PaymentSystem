﻿using System;
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

        public double Amount { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }
    }

    public class PaymentListViewModel
    {
        public string Date { get; set; }

        public double Amount { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }
    }

    public class AccountBalanceViewModel 
    { 
        public string AccountNumber { get; set; }

        public double AccountBalance { get; set; }

        public IEnumerable<PaymentListViewModel> PaymentList { get; set; }
    }
 }
