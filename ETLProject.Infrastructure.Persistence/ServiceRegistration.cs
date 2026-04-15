using ETLProject.Infrastructure.Persistence.DbContext;
using ETLProject.Infrastructure.Persistence.Extractors;
using ETLProject.Infrastructure.Persistence.Extractors.ApiExtractors;
using ETLProject.Infrastructure.Persistence.Extractors.CsvExtractors;
using ETLProject.Infrastructure.Persistence.Extractors.DbExtractors;
using ETLProject.Infrastructure.Persistence.Repositories;
using ETLProjectDW.Core.Application.Interfaces;
using ETLProjectDW.Core.Application.Transformers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ETLProject.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<VentasDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("VentasDB")));

            services.AddDbContext<VentasDbDWContext>(options =>
                options.UseSqlServer(config.GetConnectionString("VentasDB_DW")));


            services.AddTransient<CustomerCsvExtractor>();
            services.AddTransient<ProductCsvExtractor>();
            services.AddTransient<OrderCsvExtractor>();

            services.AddTransient<CustomerDbExtractor>();
            services.AddTransient<OrderDbExtractor>();
            services.AddTransient<OrderDetailDbExtractor>();
            services.AddTransient<ProductDbExtractor>();


            services.AddTransient<CustomerTransformer>();
            services.AddTransient<ProductTransformer>();
            services.AddTransient<OrderStatusTransformer>();
            services.AddTransient<TimeTransformer>();
            services.AddTransient<FactSaleTransformer>();


            services.AddTransient<IDimCustomerRepository, DimCustomerRepository>();
            services.AddTransient<IDimProductRepository, DimProductRepository>();
            services.AddTransient<IDimOrderStatusRepository, DimOrderStatusRepository>();
            services.AddTransient<IDimTimeRepository, DimTimeRepository>();
            services.AddTransient<IFactSaleRepository, FactSaleRepository>();


            services.AddHttpClient<ApiProductExtractor>();
        }
    }
}