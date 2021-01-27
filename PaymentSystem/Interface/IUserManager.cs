using PaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Interface
{
    public interface IUserManager
    {
        Users GetUser(string username);
    }
}
