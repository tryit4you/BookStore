using BookStore.Data.Interfaces;
using BookStore.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Components
{
    public class PageViewComponent:ViewComponent
    {
        private readonly IPageRepository _pageRepository;
        public PageViewComponent(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }
        public IViewComponentResult Invoke()
        {
             var model = _pageRepository.Pages().Where(x => x.Name.ToLower().Contains("footer") ||
               x.Name.ToLower().Contains("chân trang") ||
               x.Name.ToLower().Contains("cuối trang")).FirstOrDefault();
            if (model is null)
            {
                model.Content = "Không tìm thấy nội dung";
            }
            return View(model);
        }
    }
}
