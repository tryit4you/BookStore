using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class SlideViewModels
    {
        public string UrlPannel { get; set; }
        public IEnumerable<Slide> Slides { get; set; }
    }
}
