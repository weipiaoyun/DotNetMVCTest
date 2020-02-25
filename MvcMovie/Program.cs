using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MvcMovie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //IServiceCollection services = new ServiceCollection();

            //services.AddTransient<MyService>();

            //var serviceProvider = services.BuildServiceProvider();
            //var myService = serviceProvider.GetService<MyService>();

            //myService.DoIt();

            //Demo2();

            //Demo3();

            //FactoryMethodDemo();


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void FactoryMethodDemo()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<IMyServiceDependency, MyServiceDependency>();
            //Overload method for factory registration
           services.AddTransient(
               provider => new MyService(provider.GetService<IMyServiceDependency>())
           );
           //services.AddTransient<MyService>();

            var serviceProvider = services.BuildServiceProvider();
            var instance = serviceProvider.GetService<MyService>();

            instance.DoIt();
        }

        private static void Demo2()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<TransientDateOperation>();
            services.AddScoped<ScopedDateOperation>();
            services.AddSingleton<SingletonDateOperation>();

            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine();
            Console.WriteLine("-------- 1st Request --------");
            Console.WriteLine();

            var transientService = serviceProvider.GetService<TransientDateOperation>();
            var scopedService = serviceProvider.GetService<ScopedDateOperation>();
            var singletonService = serviceProvider.GetService<SingletonDateOperation>();

            Console.WriteLine();
            Console.WriteLine("-------- 2nd Request --------");
            Console.WriteLine();

            var transientService2 = serviceProvider.GetService<TransientDateOperation>();
            var scopedService2 = serviceProvider.GetService<ScopedDateOperation>();
            var singletonService2 = serviceProvider.GetService<SingletonDateOperation>();

            Console.WriteLine();
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
        }


        private static void Demo3()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<TransientDateOperation>();
            services.AddScoped<ScopedDateOperation>();
            services.AddSingleton<SingletonDateOperation>();

            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine();
            Console.WriteLine("-------- 1st Request --------");
            Console.WriteLine();

            using (var scope = serviceProvider.CreateScope())
            {
                var transientService = scope.ServiceProvider.GetService<TransientDateOperation>();
                var scopedService = scope.ServiceProvider.GetService<ScopedDateOperation>();
                var singletonService = scope.ServiceProvider.GetService<SingletonDateOperation>();
            }

            Console.WriteLine();
            Console.WriteLine("-------- 2nd Request --------");
            Console.WriteLine();

            using (var scope = serviceProvider.CreateScope())
            {
                var transientService = scope.ServiceProvider.GetService<TransientDateOperation>();
                var scopedService = scope.ServiceProvider.GetService<ScopedDateOperation>();
                var singletonService = scope.ServiceProvider.GetService<SingletonDateOperation>();
            }

            Console.WriteLine();
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
        }



    }


    public class TransientDateOperation
    {
        public TransientDateOperation()
        {
            Console.WriteLine("Transient service is created!");
        }
    }

    public class ScopedDateOperation
    {
        public ScopedDateOperation()
        {
            Console.WriteLine("Scoped service is created!");
        }
    }

    public class SingletonDateOperation
    {
        public SingletonDateOperation()
        {
            Console.WriteLine("Singleton service is created!");
        }
    }
    public class MyService
    {
        private readonly IMyServiceDependency _dependency;

        public MyService(IMyServiceDependency dependency)
        {
            _dependency = dependency;
        }

        public void DoIt()
        {
            _dependency.DoIt();
        }
    }

    public class MyServiceDependency : IMyServiceDependency
    {
        public void DoIt()
        {
            Console.WriteLine("Hello from MyServiceDependency");
        }
    }

    public interface IMyServiceDependency
    {
        void DoIt();
    }
}
