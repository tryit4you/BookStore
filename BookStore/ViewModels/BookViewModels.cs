using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class BookViewModels
    {
        public IEnumerable<Book> Books { get; set; }
        public int CountItem { get; set; }
    }
}
