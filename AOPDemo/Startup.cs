using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AOPDemo.Infrastructure.Castle;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AOPDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<ICacheProvider, MemoryCacheProvider>();

            return this.GetAutofacServiceProvider(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private IServiceProvider GetAutofacServiceProvider(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var assembly = this.GetType().GetTypeInfo().Assembly;
            builder.RegisterType<TestCachingInterceptor>();
            //scenario 1
            builder.RegisterAssemblyTypes(assembly)
                         .Where(type => typeof(ITestCaching).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                         .AsImplementedInterfaces()
                         .InstancePerLifetimeScope()
                         .EnableInterfaceInterceptors()
                         .InterceptedBy(typeof(TestCachingInterceptor));
            //scenario2
            builder.RegisterAssemblyTypes(assembly)
             .Where(type => type.Name.EndsWith("BLL", StringComparison.OrdinalIgnoreCase))
             .EnableClassInterceptors()
             .InterceptedBy(typeof(TestCachingInterceptor));

            return new AutofacServiceProvider(builder.Build());
        }
    }
}
