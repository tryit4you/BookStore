using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly IMemoryCache _cache;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryMenuViewComponent(ICategoryRepository categoryRepository, IMemoryCache cache)
        {
            _cache = cache;
            _categoryRepository = categoryRepository;

        }
        public IViewComponentResult Invoke()
        {

            var categories = new List<Category>();
            if (!_cache.TryGetValue("categoryMenu", out categories))
            {
                if (categories == null)
                {
                    categories = _categoryRepository.Categories().ToList();

                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
           .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                _cache.Set("categoryMenu", categories);
            }
            CategoryViewModels categoryViewModels = new CategoryViewModels
            {
                Categories = categories
            };
            return View(categoryViewModels);
        }


    }
}
