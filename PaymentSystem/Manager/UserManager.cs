using PaymentSystem.Interface;
using PaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Manager
{
    public class UserManager : IUserManager
    {
        private readonly DatabaseContext _DatabaseContext;

        public UserManager(DatabaseContext DatabaseContext)
        {
            _DatabaseContext = DatabaseContext;
        }

        public Users GetUser(string username)
        {
            var user = _DatabaseContext.Users.FirstOrDefault(i => i.Username == username);

            return user;
        }
    }
}
