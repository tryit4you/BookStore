using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class BookTypeRepository : IBookTypeRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public BookTypeRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<BookType> AllBookTypes() => _dbContext.BookTypes;

        public bool Delete(string id)
        {
            var booktype = _dbContext.BookTypes.FirstOrDefault(x => x.Id == id);
            if (booktype == null)
            {
                return false;
            }
            else
            {
                _dbContext.BookTypes.Remove(booktype);
                return true;
            }
        }

        public BookType GetById(string id)
        {
            return _dbContext.BookTypes.FirstOrDefault(x => x.Id == id);
        }
        public bool CheckName(string name)
        {
            var result = _dbContext.BookTypes.Where(x => x.Name.ToLower().Equals(name.ToLower())).SingleOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Post(BookType type)
        {
            type.Id = Guid.NewGuid().ToString();
            type.CreatedDate = DateTime.Now;
            var result = _dbContext.BookTypes.Add(type);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public bool Put(BookType bookType)
        {
            var result = _dbContext.BookTypes.FirstOrDefault(x => x.Id == bookType.Id);
            if (result == null)
                return false;
            else
            {
                result.Name = bookType.Name;
                result.MetaName = bookType.MetaName;
                result.ThumbnailUrl = bookType.ThumbnailUrl;
                result.Description = bookType.Description;
                result.CategoryId = bookType.CategoryId;
                return true;
            }
        }

        public void SaveChange()
        {
            _dbContext.SaveChanges();
        }

        public int CountBookTypeByCategoryId(string categoryId)
        {
            var count = _dbContext.BookTypes.Where(x => x.CategoryId == categoryId).Count();
            return count;
        }

        public IEnumerable<BookType> GetByCategory(string categoryId)=>_dbContext.BookTypes.Where(x => x.CategoryId == categoryId).ToList();

    }
}
