using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface IContactRepository
    {
        IEnumerable<Contact> Contacts();
        Contact GetContact(string id);
        bool Delete(string id); bool CheckName(string name);
        bool Update(Contact Contact);
        Contact AddContact(Contact Contact);
        void SaveChange();
    }
}
