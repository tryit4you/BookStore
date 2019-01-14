using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.Admin.ViewModels
{
    public class LoadByCategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<BookType> BookTypes { get; set; }
    }
}
