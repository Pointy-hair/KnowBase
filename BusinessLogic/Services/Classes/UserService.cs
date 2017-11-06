using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;

namespace BusinessLogic.Services.Classes
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserRepository userRepository;

        public UserService(IUnitOfWork unitOfWork, 
            IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;

            this.userRepository = userRepository;
        }

        public async Task<ICollection<User>> GetByName(string firstName, string lastName)
        {
            var username = lastName + ' ' + firstName;

            var users = await Task.Run(() => userRepository.Find(x => x.Name == username));

            return users.ToList();
        }

        public async Task<ICollection<User>> GetAllHRMs()
        {
            var result = await Task.Run(() => userRepository.Find(x => x.Role >= 2));

            return result.ToList();
        }

        public async Task<ICollection<User>> GetAllTech()
        {
            var result = await Task.Run(() => userRepository.Find(x => x.Role == 1));

            return result.ToList();
        }

        public async Task<User> GetByLogin(string login)
        {
            var allUsers = await Task.Run(() => userRepository.ReadAll());

            var user = allUsers.AsEnumerable().Where(u => string.Compare(u.Login, login, StringComparison.Ordinal) == 0);

            return user.FirstOrDefault();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
