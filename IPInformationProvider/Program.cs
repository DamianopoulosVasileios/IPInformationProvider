using IPInformationProvider.API.Caching;
using IPInformationProvider.API.DBContext;
using IPInformationProvider.API.Endpoints;
using IPInformationProvider.API.Interfaces;
using IPInformationProvider.API.Repositories;
using IPInformationProvider.API.Services;
using IPInformationProvider.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IIPService, IPService>();

//In-Memory Caching
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IIPCaching, IPCaching>();

//Endpoints
builder.Services.AddTransient<IIPInformationEndpoint, IPInformationEndpoint>();
builder.Services.AddTransient<IIPReportEndpoint, IPReportEndpoint>();

//Repositories
builder.Services.AddTransient<IIPRepository, IPRepository>();

//Hosted Services
builder.Services.AddHostedService<UpdateIPInformation>();

builder.Services.AddDbContext<IPDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")), ServiceLifetime.Singleton);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/IPInformation", async (IIPService iPService, string IPAddress) =>
{
    return await iPService.GetInformation(IPAddress);
});

app.Run();

public partial class Program { }