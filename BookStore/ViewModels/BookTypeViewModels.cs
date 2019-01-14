using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class BookTypeViewModels
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MetaName { get; set; }
        public int bookCount { get; set; }
    }
}
