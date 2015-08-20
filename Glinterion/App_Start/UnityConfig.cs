using Microsoft.Practices.Unity;
using System.Web.Http;
using Glinterion.DAL.IRepository;
using Glinterion.DAL.Repository;
using Unity.WebApi;

namespace Glinterion
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IPhotoRepository, PhotoRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IImageRepository, ImageRepository>();
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}