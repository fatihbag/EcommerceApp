using Autofac.Extensions.DependencyInjection;
using Autofac;
using EcommerceApp.Application.IoC;
using EcommerceApp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



builder.Services.AddDbContext<ECommerceAppDbContext>(_ =>
{
    _.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceConnString"));
});


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new DependencyResolver());
});







// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//�al��t�rd���mda aray�z gelsin. API'deki controller'i tetikler
    app.UseSwaggerUI();
}

app.UseStaticFiles();//wwwroot i�erisindeki dosyalar�n �al��mas�n� saaa�lar

app.UseHttpsRedirection();

app.UseAuthorization();//Yetkisi var m� yok mu?/ Yetkilendirmeyi sa�lar

app.MapControllers();

app.Run();
