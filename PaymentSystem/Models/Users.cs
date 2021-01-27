using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Models
{
    public class Users
    {
        [Key]
        public int ID { get; set; }

        public string UserCode { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Username { get; set; }

        public string AccountNumber { get; set; }
    }
}
