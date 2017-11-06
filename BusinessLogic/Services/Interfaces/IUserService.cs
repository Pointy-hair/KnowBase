using System.Collections.Generic;
using BusinessLogic.DBContext;
using System.Threading.Tasks;
using System;

namespace BusinessLogic.Services.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<ICollection<User>> GetAllHRMs();

        Task<ICollection<User>> GetAllTech();

        Task<ICollection<User>> GetByName(string firstName, string lastName);

        Task<User> GetByLogin(string login);
    }
}
