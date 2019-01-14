using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.Admin.ViewModels
{
    public class NotificationViewModels
    {
        public IEnumerable<ApplicationUser> UserRegisters { get; set; }
        public IEnumerable<Book> BooksUpload { get; set; }
    }
}
