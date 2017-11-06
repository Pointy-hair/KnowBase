using BusinessLogic.DBContext;
using BusinessLogic.Repository.AbstractRepositories;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Unit_of_Work;
using System.Threading.Tasks;
using System;

namespace BusinessLogic.Repository
{
    public class UserRepository: Repository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
