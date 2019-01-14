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
    public class CourseCategoryViewComponent:ViewComponent
    {
        private readonly ICourseCategoryRepository _courseCategoryRepository;
        private readonly IMemoryCache _cache;
        public CourseCategoryViewComponent(ICourseCategoryRepository courseCategoryRepository,IMemoryCache cache)
        {
            _cache = cache;
            _courseCategoryRepository = courseCategoryRepository;
 
        }
        public IViewComponentResult Invoke()
        {

            var categories = new List<CourseCategory>();
            if(!_cache.TryGetValue("coursecategory",out categories))
            {
                categories= _courseCategoryRepository.Categories().ToList();
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
     .SetSlidingExpiration(TimeSpan.FromMinutes(3));
            _cache.Set("coursecategory", categories, cacheEntryOptions);
            return View(categories);
        }
    }
}
