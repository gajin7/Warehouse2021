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

            //register access database as singleton
            container.RegisterSingleton<AccessDb>();

            //register repositories
            container.RegisterType<IEmployeeRepository, EmployeeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IItemRepository, ItemRepository>(new HierarchicalLifetimeManager());

            //register services
            container.RegisterType<IHashPasswordService, HashPasswordService>(new HierarchicalLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}