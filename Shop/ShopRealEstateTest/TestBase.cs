using Microsoft.Extensions.DependencyInjection;
using Shop.ApplicationServices.Services;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ShopRealEstateTest.Macros;
using Microsoft.Extensions.Hosting;
using ShopRealEstateTest.Mock;

namespace ShopRealEstateTest
{
    public abstract class TestBase
    {
        protected IServiceProvider serviceProvider { get; set; }

        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {

        }

        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();
        }

        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IRealEstateServices, RealEstateServices>();
			services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IHostEnvironment, MockIHostEnvironment>();


			services.AddDbContext<ShopContext>(x =>
            {
                x.UseInMemoryDatabase("TEST");
                x.ConfigureWarnings(e => e.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            RegisterMacros(services);
		}

        private void RegisterMacros(IServiceCollection services)
        {
            var macroBaseType = typeof(IMacros);

            var macros = macroBaseType.Assembly.GetTypes()
                .Where(x => macroBaseType.IsAssignableFrom(x) && !x.IsInterface
                && !x.IsAbstract);

            foreach (var macro in macros)
            {
                services.AddSingleton(macro);
            }
        }
    }
}
