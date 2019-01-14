using BookStore.Data.Interfaces;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Components
{
    public class Top5DownloadViewComponent : ViewComponent
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemoryCache _cache;
        public Top5DownloadViewComponent(IBookRepository bookRepository,IMemoryCache cache)
        {
            _cache = cache;
            _bookRepository = bookRepository;
        }
        public IViewComponentResult Invoke()
        {

            var items = new BookViewModels();
            if (!_cache.TryGetValue("top5download",out items)){
                if (items==null)
                {
                    var books = _bookRepository.Top5Download.Where(x => x.Status).ToList();
                    items = new BookViewModels
                    {
                        Books = books
                    };
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                _cache.Set("top5download", items,cacheEntryOptions);
            }
            return View(items);
        }
    }
}
