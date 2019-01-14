using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class CourseCategoryRepository : ICourseCategoryRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public CourseCategoryRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        } 

        public IEnumerable<CourseCategory> Categories() => _dbContext.CourseCategories;

        public bool CheckName(string name)
        {
            var result = _dbContext.CourseCategories.Where(x => x.Name.ToLower().Equals(name.ToLower())).SingleOrDefault();
            if (result!=null)
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
            var category = _dbContext.CourseCategories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return false;
            }
            else
            {
                _dbContext.CourseCategories.Remove(category);
                return true;
            }
        }

        public CourseCategory GetById(string id)
        {
            return _dbContext.CourseCategories.FirstOrDefault(x => x.Id == id);
        }

        public string GetIdByName(string name)
       => _dbContext.CourseCategories.Where(x => x.Name == name).SingleOrDefault().Id.ToString();

        public bool Post(CourseCategory CourseCategory)
        {
            CourseCategory.Id = Guid.NewGuid().ToString();
            CourseCategory.CreateDate = DateTime.Now;
            var result = _dbContext.CourseCategories.Add(CourseCategory);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public bool Put(CourseCategory CourseCategory)
        {
            var result = _dbContext.CourseCategories.FirstOrDefault(x => x.Id == CourseCategory.Id);
            if (result == null)
                return false;
            else
            {
                result.Name = CourseCategory.Name;
                result.MetaName = CourseCategory.MetaName;
                result.Status = CourseCategory.Status;
                return true;
            }
        }

        public void SaveChange()
        {
            _dbContext.SaveChanges();
        }
    }
}
