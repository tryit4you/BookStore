using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class UpdateBookViewModels
    {
        public ApplicationUser User { get; set; }
        public string BookId { get; set; }
    }
}
