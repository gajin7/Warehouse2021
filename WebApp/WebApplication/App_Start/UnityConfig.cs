using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using WebApplication.Repositories;

namespace WebApplication
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IEmployeeRepository, EmployeeRepository>(new HierarchicalLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}