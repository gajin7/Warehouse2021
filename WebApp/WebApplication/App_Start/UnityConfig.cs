using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using WebApplication.Repositories;
using WebApplication.Services;

namespace WebApplication
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            //register repositories
            container.RegisterType<IEmployeeRepository, EmployeeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IItemRepository, ItemRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IWarehouseRepository, WarehouseRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IShelfRepository, ShelfRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IReceiptRepository, ReceiptRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IVehicleRepository, VehicleRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IItemTypesRepository, ItemTypesRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICompaniesRepository, CompaniesRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportRepository, ReportRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILoadRepository, LoadRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IRampRepository, RampRepository>(new HierarchicalLifetimeManager());


            //register services
            container.RegisterType<IHashPasswordService, HashPasswordService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISearchService, SearchService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmailService, EmailService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDefaultPasswordGenerator, DefaultPasswordGenerator>(new HierarchicalLifetimeManager());
            container.RegisterType<IDocumentCreatorService, DocumentCreatorService>(new HierarchicalLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}