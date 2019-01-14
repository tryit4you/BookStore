using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _dbContext;

        public BookRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Book> Books => _dbContext.Books;

        public IEnumerable<Book> Top5Download => _dbContext.Books.OrderByDescending(x => x.CountDownload).Take(5);

        public Book GetById(string id) => _dbContext.Books.FirstOrDefault(x => x.Id == id);

        public bool Post(Book book)
        {
            book.Id = Guid.NewGuid().ToString();
            book.CreatedDate = DateTime.Now;
            var result = _dbContext.Books.Add(book);
            if (result != null)
            {
                return true;
            }
            return false;
        }
        public bool Put(Book book)
        {
            var result = _dbContext.Books.FirstOrDefault(x => x.Id == book.Id);
            if (result == null)
                return false;
            else
            {
                result.Name = book.Name;
                result.MetaName = book.MetaName;
                result.Authors = book.Authors;
                result.BookTypeId = book.BookTypeId;
                result.Cappacity = book.Cappacity;
                result.CategoryId = book.CategoryId;
                result.DateRelease = book.DateRelease;
                result.Language = book.Language;
                result.FormatDownload = book.FormatDownload;
                result.TextReference = book.TextReference;
                result.LinkReference = book.LinkReference;
                result.PageNumber = book.PageNumber;
                result.LongDescription = book.LongDescription;
                result.ThumbnailUrl = book.ThumbnailUrl;
                result.LastModifiedDate = DateTime.Now;
                return true;
            }
        }
        public bool CheckName(string name)
        {
            var result = _dbContext.Books.Where(x => x.Name.ToLower().Equals(name.ToLower())).SingleOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Delete(string id)
        {
            var book = _dbContext.Books.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return false;
            }
            else
            {
                _dbContext.Books.Remove(book);
                return true;
            }
        }

        public IQueryable<Book> QueryBooks() => _dbContext.Books.AsNoTracking().AsQueryable();
        public void SaveChange() => _dbContext.SaveChanges();

        public Book GetBook(string id) => _dbContext.Books.FirstOrDefault(x => x.Id == id);

        public IEnumerable<Book> RelatedBooks(string id)
        {
            var book = GetById(id);
            var bookName = book.MetaName;
            IEnumerable<string> splitMetaName = new List<string>();
            splitMetaName = bookName.Split('-');
            var relatedBook = _dbContext.Books.Where(x => x.Id != id && x.BookTypeId == book.BookTypeId).ToList();
            List<Book> books = new List<Book>();

            foreach (var name in splitMetaName)
            {
                var related = relatedBook.Where(x => x.MetaName.Contains(name));
                if (related.Count() > 0)
                {
                    books.AddRange(related);
                }
            }
            return books.Distinct().OrderBy(x => x.CountDownload).Take(8);
        }

        public int CountBookByBooktypeId(string booktypeId)
        {
            var countBooks = _dbContext.Books.Where(x => x.BookTypeId == booktypeId).Count();
            return countBooks;
        }

        public IQueryable<Book> GetBooksByCategory(string categoryId) => _dbContext.Books.Where(x => x.CategoryId == categoryId).AsQueryable();

        public IQueryable<Book> GetBooksByType(string typeId) => _dbContext.Books.Where(x => x.BookTypeId == typeId).AsQueryable();
    }
}
