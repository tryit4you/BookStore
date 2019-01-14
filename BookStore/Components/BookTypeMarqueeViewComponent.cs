using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Components
{
    public class BookTypeMarqueeViewComponent : ViewComponent
    {
        private readonly IBookTypeRepository _bookTypeRepository;
        private readonly IMemoryCache _caches;
        public BookTypeMarqueeViewComponent(IBookTypeRepository bookTypeRepository,IMemoryCache cache)
        {
            _caches = cache;
            _bookTypeRepository = bookTypeRepository;
        }

        public IViewComponentResult Invoke()
        {

            var bookType = new List<BookType>();
            if (!_caches.TryGetValue("booktypeMaquee", out bookType))
            {
                if (bookType== null)
                {
                    bookType = _bookTypeRepository.AllBookTypes().Where(x => x.Status == true).OrderBy(x => x.Name).ToList();

                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
           .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                _caches.Set("booktypeMaquee", bookType);
            }
            return View(bookType);
        }
    }
}