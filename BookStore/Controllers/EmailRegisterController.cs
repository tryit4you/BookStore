using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class EmailRegisterController : Controller
    {
        private readonly IEmailRegisterRepository emailRepository;
        public EmailRegisterController(IEmailRegisterRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }
        [HttpPost]
        public IActionResult Register(string email)
        {
            var model = new EmailRegister
            {
                Id = Guid.NewGuid().ToString(),
                Email = email
            };
            emailRepository.Post(model);
            emailRepository.SaveChange();
            return Redirect("/");
        }
    }
}