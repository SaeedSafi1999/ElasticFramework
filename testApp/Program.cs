using ElasticSearchSharp.Services.DependencyInjection;
using ElasticSearchSharp.Services.Services.Elastic;
using testApp.Dependency;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//set elastic
builder.Services.AddElasticFramework(new SharedDomain.Configuration.ElasticConfig
{
    Password = "S@eedS@fi1234",
    Port = 9200,
    Url = "http://localhost",
    Username = "elastic"
});
builder.Services.AddScoped<IElasticContext, ProductContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
