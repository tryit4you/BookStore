using BookStore.Data.Models;
using BookStore.Services;
using BookStore.SharedComponents;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Components
{
    public class UserHeaderViewComponent:ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        public IConfiguration Configuration { get; }
        public UserHeaderViewComponent(UserManager<ApplicationUser> user,IConfiguration configuration)
        {
            userManager = user;
        }
        public IViewComponentResult Invoke()
        {
            string key = Key.secretsKey;
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;
                string userId = userManager.Users.Where(x => x.UserName == userName).FirstOrDefault().Id;
                var userIdEncrypt = CryptorService.EncryptString(userId, key);
                UserHeaderViewModel user = new UserHeaderViewModel
                {
                    UserId = userIdEncrypt,
                    UserName = userName
                };
                return View(user);
            }
            else
            {
                return View();
            }
            
        }
    }
}
