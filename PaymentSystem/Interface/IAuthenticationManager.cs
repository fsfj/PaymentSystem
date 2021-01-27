using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Interface
{
    public interface IAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
}
