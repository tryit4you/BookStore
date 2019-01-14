using BookStore.Areas.Admin.ViewModels;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.Admin.Components
{
    public class HomeStatisticViewComponent:ViewComponent
    {
        private readonly IBookRepository _bookRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookTypeRepository _bookTypeRepository;
        public HomeStatisticViewComponent(IBookTypeRepository bookTypeRepository,
            UserManager<ApplicationUser> userManager,
            IFeedbackRepository feedbackRepository,
            ICategoryRepository categoryRepository,
            IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
            _userManager = userManager;
            _feedbackRepository = feedbackRepository;
            _categoryRepository = categoryRepository;
            _bookTypeRepository = bookTypeRepository;
        }
        private EbookVm GetEbookVm()
        {
            var booksCount = _bookRepository.Books.Count();
            var bookToday = (from b in _bookRepository.Books
                             where DateTime.Now.Subtract(b.CreatedDate).Days <= 0
                             select b
                             ).Count();
            var bookVm = new EbookVm
            {
                Total=booksCount,
                InToday=bookToday
            };
            return bookVm;
        }
        private FeedbackVm GetFeedbackVm()
        {
            var feedbackCount = _feedbackRepository.Feedbacks().Count();
            var feedbackToday = (from b in _feedbackRepository.Feedbacks()
                                 where DateTime.Now.Subtract(b.CreatedDate).Days <= 0
                                 select b
                             ).Count();
            var feedbackVm = new FeedbackVm
            {
                Total=feedbackCount,
                InToday=feedbackToday
            };
            return feedbackVm;
        }
        private Category_TypeVm Category_TypeVm()
        {
            var categoryCount = _categoryRepository.Categories().Count();
            var bookTypeCount = _bookTypeRepository.AllBookTypes().Count();
            var category_TypeVm = new Category_TypeVm
            {
                Category=categoryCount,
                Type=bookTypeCount
            };
            return category_TypeVm;
        }
        private AccountVm GetUserVm()
        {
            var accountCount = _userManager.Users.Count();
            var registerToday = (from u in _userManager.Users
                                 where DateTime.Now.Subtract(u.CreatedDate).Days <= 0
                                 select u
                             ).Count();
            var accountVm = new AccountVm
            {
                Total=accountCount,
                RegisterToday=registerToday
            };
            return accountVm;
        }
        public IViewComponentResult Invoke()
        {

            var homeView = new HomeViewModels
            {
                EbookVm = GetEbookVm(),
                AccountVm = GetUserVm(),
                FeedbackVm = GetFeedbackVm(),
                Category_TypeVm = Category_TypeVm()
            };
            return View(homeView);
        }
    }
}
