using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface IBookTypeRepository
    {
        IEnumerable<BookType> AllBookTypes();
        BookType GetById(string id);

        bool Post(BookType bookType);
        bool Put(BookType bookType);
        bool Delete(string id);
        bool CheckName(string name);
        IEnumerable<BookType> GetByCategory(string categoryId);

        int CountBookTypeByCategoryId(string categoryId);
        void SaveChange();
    }
}
