using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Models;
namespace BookStore.ViewModels
{
    public class BookDetailViewModel
    {
        public Book BookDetail { get; set; }
        public BookType BookType { get; set; }
        public DownloadFormat DownloadLinks { get; set; }
        public IEnumerable<Book> RelatedBooks { get; set; }
    }
}
