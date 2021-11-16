using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToDo.Services;
using ToDo.ViewModels;


namespace ToDo.IoC
{
    internal static class IocContainer
    {
        public static IContainer Container { get; set; }

        public static void Init()
        {
            var builder = new ContainerBuilder();

            // View Model
            builder.RegisterType<MainWindowViewModel>();

            // Services
            //builder.RegisterType<TodoItemService>().As<ITodoItemService>();
            //builder.RegisterType<DateTimeService>().As<IDateTimeService>();
            builder.RegisterAssemblyTypes(Assembly.Load("ToDo"))
                .Where(t => t.Namespace.Contains("Services"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            // Container
            Container = builder.Build();
        }
    }
}
