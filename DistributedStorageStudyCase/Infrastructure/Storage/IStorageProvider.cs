using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedStorageStudyCase.Infrastructure.Storage
{
    public interface IStorageProvider
    {
        string Name { get; }
        Task SaveAsync(string key, byte[] data);
        Task<byte[]> ReadAsync(string key);
    }
}
