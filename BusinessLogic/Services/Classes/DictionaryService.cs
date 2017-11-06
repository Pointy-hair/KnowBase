using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Classes
{
    public class DictionaryService<T> : IDictionaryService<T> where T : class
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IDictionaryRepository<T> dictionaryRepository;

        public DictionaryService(IUnitOfWork unitOfWork, 
            IDictionaryRepository<T> dictionaryRepository)
        {
            this.unitOfWork = unitOfWork;

            this.dictionaryRepository = dictionaryRepository;
        }

        public async Task<ICollection<T>> GetAll()
        {
            var result = await Task.Run(() => dictionaryRepository.ReadAll());

            return result.ToList();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
