using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface ICourseRepository
    {
        IEnumerable<Course> Courses();
        Course GetCourse(string courseId);
        bool Post(Course course);
        IEnumerable<Course> GetByCategory(string cateId);
        bool Put(Course course);
        bool Delete(string Id);
        bool CheckName(string name);
        void SaveChange();
    }
}
