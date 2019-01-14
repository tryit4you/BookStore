using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemoryCache _caches;
        public CategoryViewComponent(ICategoryRepository categoryRepository, IMemoryCache cache)
        {
            _caches = cache;
            _categoryRepository = categoryRepository;

        }
        public IViewComponentResult Invoke()
        {
            var categoryViewModels = new CategoryViewModels();
            if (!_caches.TryGetValue("categoriesVm", out categoryViewModels))
            {
                if (categoryViewModels == null)
                {
                    var categories = _categoryRepository.Categories().ToList();

                    foreach (var category in categories)
                    {
                        category.SummaryBook = _categoryRepository.CountBooks(category.Id);
                    }
                     categoryViewModels = new CategoryViewModels
                    {
                        Categories = categories
                    };
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                _caches.Set("categoriesVm", categoryViewModels, cacheEntryOptions);

            }


          
            return View(categoryViewModels);
        }
    }
}
