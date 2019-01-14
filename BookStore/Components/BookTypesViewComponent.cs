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
    public class BookTypesViewComponent:ViewComponent
    {
        private IBookTypeRepository _bookType;
        private IBookRepository _book;
        private readonly IMemoryCache _caches;
        public BookTypesViewComponent(IBookTypeRepository bookType,IBookRepository book, IMemoryCache cache)
        {
            _caches = cache;
            _bookType = bookType;
            _book = book;
        }
        public IViewComponentResult Invoke()
        {
            var bookType = new List<BookTypeViewModels>();
            if (!_caches.TryGetValue("bookType",out bookType))
            {
                if (bookType==null)
                {
                    bookType= (from type in _bookType.AllBookTypes()
                               select new BookTypeViewModels
                               {
                                   Name = type.Name,
                                   MetaName = type.MetaName,
                                   bookCount = _book.Books.Where(x => x.BookTypeId == type.Id).Count(),
                                   Id = type.Id
                               }).ToList();
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
       .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                _caches.Set("bookType", bookType,cacheEntryOptions);
                
            }
            

            return View(bookType);
        }
    }
}
