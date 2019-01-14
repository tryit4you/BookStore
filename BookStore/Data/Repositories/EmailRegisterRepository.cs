using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class EmailRegisterRepository : IEmailRegisterRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public EmailRegisterRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<EmailRegister> EmailRegisters => _dbContext.EmailRegisters.ToList();

        public bool Post(EmailRegister model) {
            
            var result = _dbContext.Add(model).Entity;
            if (result!=null)
            {
                return true;
            }
            return false;
        }
        public bool Delete(string id)
        {
            try
            {
                var email = _dbContext.EmailRegisters.FirstOrDefault(x => x.Id == id);
                _dbContext.EmailRegisters.Remove(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void SaveChange()
        {
            _dbContext.SaveChanges();
        }

        public EmailRegister GetEmail(string id)
        =>_dbContext.EmailRegisters.FirstOrDefault(x => x.Id == id);

    }
}
