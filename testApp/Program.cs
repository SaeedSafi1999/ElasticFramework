using Elastic.Clients.Elasticsearch;
using ElasticFramework.DependencyInjection;
using ElasticFramework.Services.Elastic;
using System;
using System.Reflection;
using testApp.Dependency;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

//set elastic
builder.Services.AddScoped<IElasticContext, ProductContext>();


//add Assemblies tha inheritance from IElasticContext
List<Assembly> assemblies = new();
assemblies.Add(Assembly.GetEntryAssembly());//add more if you need
//add ElasticFramework
builder.Services.AddElasticFramework(op =>
{
    op.Password = "S@eedS@fi1234";
    op.Port = 9200;
    op.Url = "http://localhost";
    op.Username = "elastic";
    op.assemblies = assemblies.ToArray();
});

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
