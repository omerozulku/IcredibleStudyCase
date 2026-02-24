using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedStorageStudyCase.Infrastructure.Chunking
{
    public class Chunker
    {
        public IEnumerable<byte[]> Split(string filePath, int chunkSize)
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var buffer = new byte[chunkSize];

            int read;
            while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                yield return buffer.Take(read).ToArray();
            }
        }
    }
}
