using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TinyUrlApp.DataAccess;
using TinyUrlApp.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IURLService, URLService>();
        services.AddScoped <IRepository, Repository>();

    })
    .Build();

var urlService = host.Services.GetRequiredService<IURLService>();

var shortUrl = urlService.CreateShortURLAsync("https://www.example.com", "exmpl");
Console.WriteLine("Short URL: " + shortUrl);

var longUrl = urlService.GetLongURLAsync("exmpl");
Console.WriteLine("Long URL: " + longUrl);

var longUrla = urlService.GetLongURLAsync("exmpl");


var accessCount = urlService.GetAccessCount("exmpl");
Console.WriteLine("Access Count: " + accessCount);