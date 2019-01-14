using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.Admin.ViewModels
{
    public class ApproveEbooksViewModels
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
