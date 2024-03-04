using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TesteApi;
using TesteApi.Models;
using TesteApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ProductService>();

// Leitura das configurações do arquivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<Context>(options =>
{
    options.UseInMemoryDatabase("TestDatabase");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Descomentar para 
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Inicialize o banco de dados e adicione produtos aleatórios
app.SeedData();

app.Run();
