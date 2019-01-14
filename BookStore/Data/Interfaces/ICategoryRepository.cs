using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories();
        Category GetById(string id);
        bool Post(Category category);
        bool Put(Category category);
        bool Delete(string id);
        bool CheckName(string name);
        int CountBooks(string categoryId);
        //this method for SeedData
        string GetIdByName(string name);
        void SaveChange();

    }
}
