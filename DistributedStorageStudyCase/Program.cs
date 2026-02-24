using DistributedStorageStudyCase.Application.Interfaces;
using DistributedStorageStudyCase.Application.Services;
using DistributedStorageStudyCase.Application.Strategies;
using DistributedStorageStudyCase.Infrastructure.Hashing;
using DistributedStorageStudyCase.Infrastructure.Metadata;
using DistributedStorageStudyCase.Infrastructure.Storage;
using DistributedStorageStudyCase.Persistence.Context;
using DistributedStorageStudyCase.Application.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Configuration;
var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: true);
});




services.AddDbContext<MetadataDbContext>(options =>
{
    options.UseSqlite("Data Source=metadata.db");
});
// Application
services.AddSingleton<IFileUploadService, FileUploadService>();
services.AddSingleton<IFileDownloadService, FileDownloadService>();

// Strategies
services.AddSingleton<IChunkSizeStrategy, DefaultChunkSizeStrategy>();

services.AddSingleton<IHashService, Sha256HashService>();
services.AddSingleton<IStorageProvider, FileSystemStorageProvider>();
services.AddSingleton<IStorageProvider, DatabaseStorageProvider>();
services.AddSingleton<IMetadataRepository, MetadataRepository>();


var provider = services.BuildServiceProvider();

var uploadService = provider.GetRequiredService<IFileUploadService>();
var downloadService = provider.GetRequiredService<IFileDownloadService>();
var metadataRepo = provider.GetRequiredService<IMetadataRepository>();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("==== Lütfen yapmak istediğiniz işlemi seçiniz ====");
    Console.WriteLine("1) Dosya Yükle");
    Console.WriteLine("2) Dosya İndir");
    Console.WriteLine("0) Çıkış");
    Console.Write("Seçiminiz: ");

    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            await UploadMenu.Draw(uploadService);
            break;

        case "2":
            await DownloadMenu.Draw(downloadService, metadataRepo);
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Geçersiz seçim.");
            break;
    }
}

