using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class ContactController : Controller
    {
        private readonly IPageRepository _pageRepository;
        public IActionResult Index()
        {
            var model = _pageRepository.Pages().Where(x => x.Name.ToLower().Contains("liên hệ")||
            x.Name.Contains("contact")||
            x.Name.Contains("lien he")||
            x.Name.Contains("lienhe")).FirstOrDefault();
            if (model is null)
            {
                model.Content = "Không tìm thấy nội dung";
            }

            return View(model);
        }
        public ContactController(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }
    }
}