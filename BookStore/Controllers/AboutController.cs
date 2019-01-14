using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AboutController : Controller
    {
        private readonly IPageRepository _pageRepository;
        public IActionResult Index()
        {
            var model = _pageRepository.Pages().Where(x => x.Name.ToLower().Contains("thong tin") ||
              x.Name.ToLower().Contains("thông tin") ||
              x.Name.ToLower().Contains("thong-tin") ||
              x.Name.ToLower().Contains("about")).FirstOrDefault();
            if (model is null)
            {
                model.Content="Không tìm thấy nội dung";
            }
            return View(model);
        }
        public AboutController(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }
    }
}