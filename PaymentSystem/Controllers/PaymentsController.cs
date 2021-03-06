﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Interface;
using PaymentSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IUserManager _userManager;

        public PaymentsController(IPaymentManager paymentManager, IUserManager usermanager)
        {
            _paymentManager = paymentManager;
            _userManager = usermanager;
        }

        [HttpGet("getpayments")]
        public async Task<ActionResult> GetPayments()
        {
            var user = await Task.Run(() => _userManager.GetUser(User.Identity.Name)); //* i did this instead of joining users and payments user so that you can display the name of user if you want to * 

            var paymentLists = await Task.Run(() => _paymentManager.GetPaymentList(user.UserCode)
            .Select(i => new PaymentListViewModel
            {
                Date = i.Date.ToString("yyyy-MM-dd hh:mm tt"),
                Amount = i.Amount,
                Status = i.Status,
                Reason = i.Reason
            }));

            AccountBalanceViewModel accountBalanceViewModel = new AccountBalanceViewModel
            {
                AccountNumber = user.AccountNumber, // I assume that you need to look into account number if you want to see your paymentdetails.
                AccountBalance = paymentLists.Sum(i => i.Amount), // * I assume that you need to get the sum of payments to get the Account balance of user. *
                PaymentList = paymentLists,
            };

            return Ok(accountBalanceViewModel); 
        }
    }
}
