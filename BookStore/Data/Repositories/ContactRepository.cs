using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class ContactRepository:IContactRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public ContactRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Contact AddContact(Contact Contact)
        {
            Contact.Id = Guid.NewGuid().ToString();
            return _dbContext.Contacts.Add(Contact).Entity;
        }

        public bool Delete(string id)
        {
            try
            {
                var Contact = _dbContext.Contacts.FirstOrDefault(x => x.Id == id);
                _dbContext.Contacts.Remove(Contact);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CheckName(string name)
        {
            var result = _dbContext.Contacts.Where(x => x.Name.ToLower().Equals(name.ToLower())).SingleOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Contact GetContact(string id)=> _dbContext.Contacts.FirstOrDefault(x => x.Id == id);

        public IEnumerable<Contact> Contacts() => _dbContext.Contacts.ToList();

        public bool Update(Contact Contact)
        {
            try
            {
                var _Contact = _dbContext.Contacts.FirstOrDefault(x => x.Id == Contact.Id);
                _Contact.Name = Contact.Name;
                _Contact.Phone = Contact.Phone;
                _Contact.Website = Contact.Website;
                _Contact.Address = Contact.Address;
                _Contact.Content= Contact.Content;
                _Contact.Email= Contact.Email;
                _Contact.Status = Contact.Status;
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public void SaveChange() => _dbContext.SaveChanges();

    }
}
