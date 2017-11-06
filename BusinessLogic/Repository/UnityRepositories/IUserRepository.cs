using BusinessLogic.DBContext;
using BusinessLogic.Repository.IRepositories;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.UnityRepositories
{
    public interface IUserRepository: IFindRepository<User>, IReadRepository<User>
    {
    }
}
