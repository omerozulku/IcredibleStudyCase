using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedStorageStudyCase.Infrastructure.Storage
{
    public class DatabaseStorageProvider : IStorageProvider
    {
        public string Name => throw new NotImplementedException();

        public Task<byte[]> ReadAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(string key, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
