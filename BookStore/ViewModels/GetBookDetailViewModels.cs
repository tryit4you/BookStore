using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class GetBookDetailViewModels
    {
        public Book Book { get; set; }
        public DownloadFormat DownloadLinks { get; set; }
    }
}
