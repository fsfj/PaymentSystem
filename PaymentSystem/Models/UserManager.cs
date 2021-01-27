using PaymentSystem.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Models
{
    public class UserManager : IUserManager
    {
        private readonly DatabaseContext _DatabaseContext;

        public UserManager(DatabaseContext DatabaseContext)
        {
            _DatabaseContext = DatabaseContext;
        }

        //private readonly IList<Users> users = new List<Users>
        //{
        //    new Users { ID = 1, Username = "usertest1", UserCode = "USER0001", AccountNumber = "000001", LastName = "LName1", FirstName = "FName1", MiddleName = "MName1" },
        //    new Users { ID = 2, Username = "usertest2", UserCode = "USER0002", AccountNumber = "000002", LastName = "LName2", FirstName = "FName2", MiddleName = "MName2" }
        //};


        public Users GetUser(string username)
        {
            var user = _DatabaseContext.Users.FirstOrDefault(i => i.Username == username); //users.FirstOrDefault(i => i.Username == username);

            return user;
        }
    }
}
