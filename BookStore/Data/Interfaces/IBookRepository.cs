using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }
        IQueryable<Book> QueryBooks();
        IQueryable<Book> GetBooksByCategory(string categoryId);
        IQueryable<Book> GetBooksByType(string typeId);
        IEnumerable<Book> RelatedBooks(string id);
        IEnumerable<Book> Top5Download { get; }
        Book GetById(string id);
        bool Post(Book book);
        bool Put(Book book);
        bool Delete(string id);
        Book GetBook(string id);
        int CountBookByBooktypeId(string booktypeId);
        bool CheckName(string name);
        void SaveChange();
    }
}
