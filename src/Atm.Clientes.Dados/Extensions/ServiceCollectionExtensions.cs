using Atm.Clientes.Dados.Repositories;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Atm.Clientes.Dados.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void SetupRepositories(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IRepository<Domain.Fornecedor>), typeof(Repository<Domain.Fornecedor>));
            //services.AddScoped(typeof(IRepository<Produto>), typeof(Repository<Produto>));
        }

        internal static void SetupDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DbContext>(options =>
                options.EnableSensitiveDataLogging()
                       .UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(DbContext).Assembly.FullName))
            );
        }
    }
}
