using ElasticSearchSharp.Services.Services.Elastic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedDomain.Attributes;
using SharedDomain.Configuration;



//Entity//////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Represents a product document in Elasticsearch.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets or sets the ID of the product.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Gets or sets the category of the product.
    /// </summary>
    public string Category { get; set; }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////



//EntityContext//////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Context class for interacting with Elasticsearch for product-related operations.
/// </summary>
public class ProductContext : ElasticContext
{
    public ProductContext(ElasticConfig config) : base(config) { }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////



//interface of entity repository////////////////////////////////////////////////////////////////////////
/// <summary>
/// Interface for product repository.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Searches for products by category asynchronously.
    /// </summary>
    /// <param name="category">The category to search by.</param>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> SearchByCategoryAsync(string category);

    /// <summary>
    /// Searches for products by price range asynchronously.
    /// </summary>
    /// <param name="minPrice">The minimum price.</param>
    /// <param name="maxPrice">The maximum price.</param>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> SearchByPriceRangeAsync(double minPrice, double maxPrice);
}
///////////////////////////////////////////////////////////////////////////////////////////////////////



// implement of EntityRepository/////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Repository class for managing product documents in Elasticsearch.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly IElasticContext _context;

    public ProductRepository(IElasticContext context)
    {
        _context = context;
    }

    [IndexName("products")]
    public virtual async Task<IEnumerable<Product>> SearchByCategoryAsync(string category)
    {
        var res =  await _context.SearchAsync<Product>("products", q => q.Term(t => t.Field(f => f.Category).Value(category)));
        return res;
    }

    public virtual async Task<IEnumerable<Product>> SearchByPriceRangeAsync(double minPrice, double maxPrice)
    {
        return await _context.SearchAsync<Product>("products", q => q.Range(r => r.Field(f => f.Price).GreaterThanOrEquals(minPrice).LessThanOrEquals(maxPrice)));
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////




class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // Resolve the ProductRepository
        var productRepository = host.Services.GetRequiredService<IProductRepository>();

        // Create sample products
        var products = new[]
        {
            new Product { Id = Guid.NewGuid().ToString(), Name = "Product 1", Price = 10.0, Category = "Category A" },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Product 2", Price = 11.0, Category = "Category A" },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Product 3", Price = 12.0, Category = "Category B" },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Product 4", Price = 100.0, Category = "Category A" },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Product 5", Price = 101.0, Category = "Category B" },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Product 6", Price = 20.1, Category = "Category B" },
            new Product { Id = Guid.NewGuid().ToString(), Name = "Product 7", Price = 15.0, Category = "Category A" }
        };

        var context = host.Services.GetRequiredService<IElasticContext>();

        // Create the index and index sample products
        await context.CreateIndexAsync("products");
        foreach (var product in products)
        {
            var res = await context.IndexDocumentAsync("products", product);
        }

        // Search by category
        var categoryAProducts = await productRepository.SearchByCategoryAsync("Category A");
        Console.WriteLine("Products in Category A:");
        foreach (var product in categoryAProducts)
        {
            Console.WriteLine($"{product.Name} - {product.Price}");
        }

        var categoryBProducts = await productRepository.SearchByCategoryAsync("Category B");
        Console.WriteLine("Products in Category B:");
        foreach (var product in categoryBProducts)
        {
            Console.WriteLine($"{product.Name} - {product.Price}");
        }

        // Search by price range
        var priceRangeProducts = await productRepository.SearchByPriceRangeAsync(10, 20);
        Console.WriteLine("Products in price range 10-20:");
        foreach (var product in priceRangeProducts)
        {
            Console.WriteLine($"{product.Name} - {product.Price}$");
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                // Configure ElasticConfig from appsettings.json
                var elasticConfig = new ElasticConfig
                {
                    Password = "S@eedS@fi1234",
                    Port = 9200,
                    Url = "http://localhost",
                    Username = "elastic"
                };
                services.AddSingleton(elasticConfig);

                // Register ElasticContext and ProductRepository
                services.AddScoped<IElasticContext, ProductContext>();
                services.AddScoped<IProductRepository, ProductRepository>();

                // Register other necessary services
            });
}
