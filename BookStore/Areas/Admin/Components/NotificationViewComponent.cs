using BookStore.Areas.Admin.ViewModels;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookStore.Areas.Admin.Components
{
    public class NotificationViewComponent:ViewComponent
    {
        private UserManager<ApplicationUser> userManager;
        private IBookRepository bookRepository;
        public NotificationViewComponent(UserManager<ApplicationUser> userManager,IBookRepository bookRepository)
        {
            this.userManager = userManager;
            this.bookRepository = bookRepository;
        }
        public IViewComponentResult Invoke()
        {
            var getUserRegisterToDay = (from user in userManager.Users
                                       where DateTime.Now.Subtract(user.CreatedDate).Days <= 0
                                       orderby user.CreatedDate descending
                                       select user).Take(10).ToList();
            var getBookUploadToDay = (from book in bookRepository.Books
                                      where DateTime.Now.Subtract(book.CreatedDate).Days <= 0
                                      orderby book.CreatedDate descending
                                      select book).Take(10).ToList();
            var NotificationViewModels = new NotificationViewModels
            {
                UserRegisters = getUserRegisterToDay,
                BooksUpload=getBookUploadToDay
            };
            return View(NotificationViewModels);
        }
    }
}
