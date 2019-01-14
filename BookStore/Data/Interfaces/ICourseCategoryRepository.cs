using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface ICourseCategoryRepository
    {
        IEnumerable<CourseCategory> Categories();
        CourseCategory GetById(string id);
        bool Post(CourseCategory category);
        bool Put(CourseCategory category);
        bool Delete(string id);
        bool CheckName(string name);
        //this method for SeedData
        string GetIdByName(string name);
        void SaveChange();

    }
}
