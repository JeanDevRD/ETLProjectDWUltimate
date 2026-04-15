using ETLProject.Infrastructure.Persistence.Extractors;
using ETLProject.Infrastructure.Persistence.Extractors.ApiExtractors;
using ETLProject.Infrastructure.Persistence.Extractors.CsvExtractors;
using ETLProject.Infrastructure.Persistence.Extractors.DbExtractors;
using ETLProjectDW.Core.Application.Transformers;
using ETLProjectDW.Core.Application.Interfaces;
using System.Text.Json;

namespace Worker
{
    public class ExtractionWorker : BackgroundService
    {
        private readonly ILogger<ExtractionWorker> _logger;
        private readonly IConfiguration _config;

        private readonly CustomerCsvExtractor _customerCsv;
        private readonly ProductCsvExtractor _productCsv;
        private readonly OrderCsvExtractor _orderCsv;
        private readonly CustomerDbExtractor _customerDb;
        private readonly OrderDbExtractor _orderDb;
        private readonly OrderDetailDbExtractor _orderDetailDb;
        private readonly ApiProductExtractor _apiProducts;

        private readonly CustomerTransformer _customerTransformer;
        private readonly ProductTransformer _productTransformer;
        private readonly OrderStatusTransformer _statusTransformer;
        private readonly TimeTransformer _timeTransformer;
        private readonly FactSaleTransformer _factTransformer;

        private readonly IDimCustomerRepository _customerRepo;
        private readonly IDimProductRepository _productRepo;
        private readonly IDimOrderStatusRepository _statusRepo;
        private readonly IDimTimeRepository _timeRepo;
        private readonly IFactSaleRepository _factRepo;

        public ExtractionWorker(
            ILogger<ExtractionWorker> logger,
            IConfiguration config,

            CustomerCsvExtractor customerCsv,
            ProductCsvExtractor productCsv,
            OrderCsvExtractor orderCsv,
            CustomerDbExtractor customerDb,
            OrderDbExtractor orderDb,
            OrderDetailDbExtractor orderDetailDb,
            ApiProductExtractor apiProducts,

            CustomerTransformer customerTransformer,
            ProductTransformer productTransformer,
            OrderStatusTransformer statusTransformer,
            TimeTransformer timeTransformer,
            FactSaleTransformer factTransformer,

            IDimCustomerRepository customerRepo,
            IDimProductRepository productRepo,
            IDimOrderStatusRepository statusRepo,
            IDimTimeRepository timeRepo,
            IFactSaleRepository factRepo)
        {
            _logger = logger;
            _config = config;

            _customerCsv = customerCsv;
            _productCsv = productCsv;
            _orderCsv = orderCsv;
            _customerDb = customerDb;
            _orderDb = orderDb;
            _orderDetailDb = orderDetailDb;
            _apiProducts = apiProducts;

            _customerTransformer = customerTransformer;
            _productTransformer = productTransformer;
            _statusTransformer = statusTransformer;
            _timeTransformer = timeTransformer;
            _factTransformer = factTransformer;

            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _statusRepo = statusRepo;
            _timeRepo = timeRepo;
            _factRepo = factRepo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando proceso ETL completo: {time}", DateTimeOffset.Now);

            var staging = _config["StagingPath"]!;
            Directory.CreateDirectory(staging);

            try
            {
                _logger.LogInformation("Extrayendo datos...");

                var csvCustomers = await _customerCsv.ExtractAsync();
                var csvProducts = await _productCsv.ExtractAsync();
                var csvOrders = await _orderCsv.ExtractAsync();

                var dbCustomers = await _customerDb.ExtractAsync();
                var dbOrders = await _orderDb.ExtractAsync();
                var dbOrderDetails = await _orderDetailDb.ExtractAsync();

                var apiProducts = await _apiProducts.ExtractAsync();

                await Save(csvCustomers, Path.Combine(staging, "csv_customers.json"));
                await Save(csvProducts, Path.Combine(staging, "csv_products.json"));
                await Save(csvOrders, Path.Combine(staging, "csv_orders.json"));

                await Save(dbCustomers, Path.Combine(staging, "db_customers.json"));
                await Save(dbOrders, Path.Combine(staging, "db_orders.json"));
                await Save(dbOrderDetails, Path.Combine(staging, "db_orderdetails.json"));

                await Save(apiProducts, Path.Combine(staging, "api_products.json"));


                _logger.LogInformation("Transformando datos...");

                var dimCustomers = _customerTransformer.Transform(dbCustomers);
                var dimProducts = _productTransformer.Transform(apiProducts);
                var dimStatuses = _statusTransformer.Transform(dbOrders);
                var dimTimes = _timeTransformer.Transform(dbOrders);
                var factSales = _factTransformer.Transform(dbOrders, dbOrderDetails);

                _logger.LogInformation("Cargando dimensiones...");

                await _timeRepo.LoadAsync(dimTimes, stoppingToken);
                await _customerRepo.LoadAsync(dimCustomers, stoppingToken);
                await _productRepo.LoadAsync(dimProducts, stoppingToken);
                await _statusRepo.LoadAsync(dimStatuses, stoppingToken);

                _logger.LogInformation("Cargando tabla de hechos...");

                await _factRepo.LoadAsync(factSales, stoppingToken);

                _logger.LogInformation("Proceso ETL completado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el proceso ETL");
            }
        }

        private async Task Save<T>(IEnumerable<T> data, string path)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(path, json);
            _logger.LogInformation("Archivo guardado: {path}", path);
        }
    }
}