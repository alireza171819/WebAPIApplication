using ApplicationService.ProductServices;
using ApplicationService.ProductServices.Contracts;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Services.Contracts;
using Model.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var WebaAPIAllowSpecificOrigins = "_webAPIAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: WebaAPIAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader().WithMethods("PUT", "DELETE", "GET")
            .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<WebAPIApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebAPIApplicationConnectionString"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductApplicationService, ProductApplicationService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(WebaAPIAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
