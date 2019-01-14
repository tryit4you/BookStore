using BookStore.Data.Interfaces;
using BookStore.Data.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Components
{
    public class SlideViewComponent:ViewComponent
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IMemoryCache _cache;
        public SlideViewComponent(ISlideRepository slideRepository,IMemoryCache cache)
        {
            _cache = cache;
            _slideRepository = slideRepository;
        }
        public IViewComponentResult Invoke()
        {
            var rand = new Random();
            var slidevm = new SlideViewModels();
            if (!_cache.TryGetValue("slide_vm",out slidevm))
            {
                if (slidevm==null)
                {
                    var slide = _slideRepository.Slides();
                    var getImgThumbnail = slide.Where(x => x.IsChoose).FirstOrDefault();
                    if (getImgThumbnail is null)
                    {
                        getImgThumbnail = slide.ElementAt(rand.Next(0, slide.Count()));
                    }
                    var slideElement = slide.
                        Where(x => x.Status == true).
                        Skip(rand.Next(0, slide.Count())).
                        OrderBy(x => x.DisplayOrder).
                        Take(3).
                        ToList();
                    slidevm = new SlideViewModels
                    {
                        UrlPannel = getImgThumbnail.Image,
                        Slides = slideElement
                    };
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                _cache.Set("slide_vm", slidevm,cacheEntryOptions);
            }
           
            return View(slidevm);
        }
    }
}
