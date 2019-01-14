using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface IEmailRegisterRepository
    {
        bool Post(EmailRegister email);
        IEnumerable<EmailRegister> EmailRegisters { get; }
        EmailRegister GetEmail(string id);
        bool Delete(string id);
        void SaveChange();
    }
}
