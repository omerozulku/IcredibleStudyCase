using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedStorageStudyCase.Application.Interfaces;
using DistributedStorageStudyCase.Infrastructure.Metadata;

namespace DistributedStorageStudyCase.Application.Menus
{
    public  class DownloadMenu
    {
       public static async Task Draw(
    IFileDownloadService downloadService,
    IMetadataRepository metadataRepo)
        {
            var files = await metadataRepo.GetAllAsync();

            if (!files.Any())
            {
                Console.WriteLine("İndirilecek dosya bulunamadı.");
                return;
            }

            Console.WriteLine("İndirmek için dosya seçimi yapınız:");

            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {files[i].OriginalFileName}");
            }

            Console.Write("Seçim: ");
            if (!int.TryParse(Console.ReadLine(), out var selection))
                return;

            var file = files.ElementAtOrDefault(selection - 1);
            if (file == null)
            {
                Console.WriteLine("Geçersiz seçim.");
                return;
            }

            var targetPath = $"reconstructed_{file.OriginalFileName}";

            await downloadService.DownloadAsync(file.FileId, targetPath);

            Console.WriteLine($"İndirilen dosya: {targetPath}");
        }
    }
}
