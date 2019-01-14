using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public CategoryRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        } 

        public IEnumerable<Category> Categories() => _dbContext.Categories;

        public bool CheckName(string name)
        {
            var result = _dbContext.Categories.Where(x => x.Name.ToLower().Equals(name.ToLower())).SingleOrDefault();
            if (result!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CountBooks(string categoryId)
        {
            var summaryBook = 0;
            summaryBook = _dbContext.Books.Where(x => x.CategoryId == categoryId).Count();
            return summaryBook;
        }

        public bool Delete(string id)
        {
            var category = _dbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return false;
            }
            else
            {
                _dbContext.Categories.Remove(category);
                return true;
            }
        }

        public Category GetById(string id)
        {
            return _dbContext.Categories.FirstOrDefault(x => x.Id == id);
        }

        public string GetIdByName(string name)
       => _dbContext.Categories.Where(x => x.Name == name).SingleOrDefault().Id.ToString();

        public bool Post(Category category)
        {
            category.Id = Guid.NewGuid().ToString();
            category.CreatedDate = DateTime.Now;
            var result = _dbContext.Categories.Add(category);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public bool Put(Category category)
        {
            var result = _dbContext.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (result == null)
                return false;
            else
            {
                result.Name = category.Name;
                result.MetaName = category.MetaName;
                result.Status = category.Status;
                return true;
            }
        }

        public void SaveChange()
        {
            _dbContext.SaveChanges();
        }
    }
}
