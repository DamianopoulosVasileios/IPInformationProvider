using IPInformationProvider.API.DBContext;
using IPInformationProvider.API.Endpoints;
using IPInformationProvider.API.Interfaces;
using IPInformationProvider.API.Models;
using IPInformationProvider.API.Repositories;
using IPInformationProvider.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<UpdateCacheService>();

builder.Services.AddDbContext<IPDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

//services
builder.Services.AddTransient<IIPService, IPService>();
//repositories
builder.Services.AddTransient<IIPRepository, IPRepository>();
//models
builder.Services.AddScoped<IIPs, IPs>();
builder.Services.AddScoped<IIPResponse, IPResponse>();
//endpoints
builder.Services.AddTransient<IIPInformationEndpoint, IPInformationEndpoint>();
builder.Services.AddTransient<IIPReportEndpoint, IPReportEndpoint>();
// In-Memory Caching
builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/IPInfromation", async (IIPService iPService,string IPAddress) =>
{
    return await iPService.GetInformation(IPAddress);
});

app.Run();

public partial class Program { }

class UpdateCacheService : BackgroundService
{
    private readonly IIPService _myService;

    public UpdateCacheService(IIPService myService)
    {
        _myService = myService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _myService.UpdateIPInformation();
        }
    }
}
