using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedStorageStudyCase.Infrastructure.Storage
{
    public class FileSystemStorageProvider : IStorageProvider
    {
        public string Name => "FileSystem";

        public async Task SaveAsync(string key, byte[] data)
        {
            Directory.CreateDirectory("storage/fs");
            await File.WriteAllBytesAsync($"storage/fs/{key}", data);
        }

        public async Task<byte[]> ReadAsync(string key)
        {
            return await File.ReadAllBytesAsync($"storage/fs/{key}");
        }
    }
}
