using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public CourseRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CheckName(string name)
        {
            var result = _dbContext.Courses.Where(x => x.Name.ToLower().Equals(name.ToLower())).SingleOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Course> Courses()
        =>
            _dbContext.Courses.AsNoTracking().AsQueryable();
        

        public bool Delete(string Id)
        {
            var course = _dbContext.Courses.FirstOrDefault(x => x.Id == Id);
            if (course == null)
            {
                return false;
            }
            else
            {
                _dbContext.Courses.Remove(course);
                return true;
            }
        }

        public IEnumerable<Course> GetByCategory(string cateId) =>_dbContext.Courses.Where(x => x.CourseId == cateId).ToList();

        public Course GetCourse(string courseId)=> _dbContext.Courses.FirstOrDefault(x => x.Id == courseId);

        public bool Post(Course course)
        {
            course.Id = Guid.NewGuid().ToString();
            course.CreatedDate = DateTime.Now;
            var result = _dbContext.Courses.Add(course);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public bool Put(Course course)
        {
            var result = _dbContext.Courses.FirstOrDefault(x => x.Id == course.Id);
            if (result == null)
                return false;
            else
            {
                result.Name = course.Name;
                result.MetaName = course.MetaName;
                result.Authors = course.Authors;
                result.CourseId = course.CourseId;
                result.Description = course.Description;
                result.SharedUrl = course.SharedUrl;
                result.AvatarData = course.AvatarData;
                result.LastModifiedDate = DateTime.Now;
                result.Status = course.Status;
                result.Reference = course.Reference;
                return true;
            }
        }

        public void SaveChange()
        {
            _dbContext.SaveChanges();
        }
    }
}
