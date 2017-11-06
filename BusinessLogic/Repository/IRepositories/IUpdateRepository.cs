namespace BusinessLogic.Repository.IRepositories
{
    public interface IUpdateRepository<T> where T: class
    {
        T Update(T updatedT);
    }
}
