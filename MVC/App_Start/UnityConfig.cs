using System;
using Microsoft.Practices.Unity;
using BusinessLogic.DBContext;
using BusinessLogic.ElasticSearch;
using BusinessLogic.Repository;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Classes;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Unit_of_Work;
using MVC.Authorization;
using BusinessLogic.Repository.DictionaryRepositories;
using BusinessLogic.Helpers;
using BusinessLogic.ElasticSearch.ElasticRepository;
using BusinessLogic.ElasticSearch.ElasticRepository.IRepository;
using BusinessLogic.ElasticSearch.ElasticServices;
using BusinessLogic.ElasticSearch.ElasticServices.IElasticServices;

namespace MVC.App_Start
{
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager(),
                                                            new InjectionConstructor());

            container.RegisterType<ICandidateService, CandidateService>();

            container.RegisterType<IEventService, EventService>();

            container.RegisterType<IVacancyService, VacancyService>();

            container.RegisterType<IPrevJobsContactsService, PrevJobsContactsService>();

            container.RegisterType<IInterviewService, InterviewService>();

            container.RegisterType<IUserService, UserService>();

            container.RegisterType<IMailService, MailService>();

            container.RegisterType<IGoogleCalendarService, GoogleCalendarService>();

            container.RegisterType<INotificationService, NotificationService>();

            container.RegisterType<IFileService, FileService>();


            container.RegisterType<ICandidateRepository, CandidateRepository>();

            container.RegisterType<ITechInterviewRepository, TechInterviewRepository>();

            container.RegisterType<IUserRepository, UserRepository>();

            container.RegisterType<IVacancyRepository, VacancyRepository>();

            container.RegisterType<IGeneralInterviewRepository, GeneralInterviewRepository>();

            container.RegisterType<IEventRepository, EventRepository>();

            container.RegisterType<IContactRepository, ContactRepository>();

            container.RegisterType<IPrevJobContactsRepository, PrevJobContactsRepository>();

            container.RegisterType<INotificationRepository, NotificationRepository>();


            container.RegisterType<ObjectComparer>();

            container.RegisterType<ObjectStringifier>();


            container.RegisterType<KnowBaseDBContext>();

            container.RegisterType<ElasticSearchContext>();

            container.RegisterType<AuthorizationServerProvider>();

            container.RegisterType(typeof(IDictionaryRepository<>), typeof(DictionaryRepository<>));

            container.RegisterType(typeof(IDictionaryService<>), typeof(DictionaryService<>));

            container.RegisterType<NLog.ILogger, NLog.Logger>();
            
            container.RegisterType<ICandidateElasticRepository, CandidateElasticRepository>();
            container.RegisterType<IVacancyElasticRepository, VacancyElasticRepository>();

            container.RegisterType<ICandidateElasticService, CandidateElasticService>();
            container.RegisterType<IVacancyElasticService, VacancyElasticService>();
        }
    }
}
