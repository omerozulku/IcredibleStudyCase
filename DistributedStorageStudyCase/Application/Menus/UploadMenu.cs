using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedStorageStudyCase.Application.Interfaces;
using DistributedStorageStudyCase.Infrastructure.Metadata;

namespace DistributedStorageStudyCase.Application.Menus
{
    public  class UploadMenu
    {
        public static async Task Draw(IFileUploadService uploadService)
        {
            Console.WriteLine("Yüklemek istediğiniz dosyanın yolunu yazınız. (1' den fazla dosya yüklemek için yollar arasında virgül kullanınız. Örn: c:\\test1.pdf,c:\\test2.pdf ):");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return;

            var files = input.Split(',')
                              .Select(f => f.Trim())
                              .Where(File.Exists)
                              .ToArray();

            if (!files.Any())
            {
                Console.WriteLine("Dosya bulunamadı.");
                return;
            }

            var fileIds = await uploadService.UploadAsync(files);

            Console.WriteLine("Dosyalar yüklendi. Oluşturulan dosyaIdleri:");
            foreach (var id in fileIds)
                Console.WriteLine(id);
        }
    }
}
