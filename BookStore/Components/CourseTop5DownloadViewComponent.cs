using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Components
{
    public class CourseTop5DownloadViewComponent : ViewComponent
    {
        private readonly ICourseRepository _course;
        private readonly IMemoryCache _cache;

        public CourseTop5DownloadViewComponent(ICourseRepository course,IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _course = course;
        }

        public IViewComponentResult Invoke()
        {
            var items = new List<Course>();
            if (!_cache.TryGetValue("coursetop5download",out items))
            {
                items = _course.Courses().Where(x => x.Status).
                    OrderBy(x => x.CountDownload).
                    Take(5).
                    ToList();
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
     .SetSlidingExpiration(TimeSpan.FromMinutes(3));
            _cache.Set("coursetop5download", items, cacheEntryOptions);
            return View(items);
        }
    }
}