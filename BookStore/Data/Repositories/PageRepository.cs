using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public PageRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Page AddPage(Page footer)
        {
            footer.Id = Guid.NewGuid().ToString();
            return _dbContext.Pages.Add(footer).Entity;
        }

        public bool Delete(string id)
        {
            try
            {
                var footer = _dbContext.Pages.FirstOrDefault(x => x.Id == id);
                _dbContext.Pages.Remove(footer);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CheckName(string name)
        {
            var result = _dbContext.Pages.Where(x => x.Name.ToLower().Equals(name.ToLower())).SingleOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Page GetPage(string id) => _dbContext.Pages.FirstOrDefault(x => x.Id == id);

        public IEnumerable<Page> Pages() => _dbContext.Pages.ToList();

        public bool Update(Page footer)
        {
            try
            {
                var _footer = _dbContext.Pages.FirstOrDefault(x => x.Id == footer.Id);
                _footer.Name = footer.Name;
                _footer.Content = footer.Content;
                _footer.LastModifiedDate = footer.LastModifiedDate;
                _footer.Status = footer.Status;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SaveChange() => _dbContext.SaveChanges();

        public Page GetDefaultPage() => _dbContext.Pages.Where(x => x.Status == true).FirstOrDefault();
    }
}
