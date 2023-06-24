using System.Web.Mvc;
using TaskManagerApp.Data.Repository;
using TaskManagerApp.Data.Repository.Interfaces;
using TaskManagerApp.Data.Services;
using TaskManagerApp.Data.Services.Interface;
using Unity;
using Unity.Mvc5;

namespace TaskManagerApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<ITaskRepository, TaskRepository>();
            container.RegisterType<ITaskService, TaskService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}