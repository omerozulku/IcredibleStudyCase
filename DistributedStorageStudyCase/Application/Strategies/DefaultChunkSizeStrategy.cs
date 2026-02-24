using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedStorageStudyCase.Application.Interfaces;

namespace DistributedStorageStudyCase.Application.Strategies
{
    public class DefaultChunkSizeStrategy : IChunkSizeStrategy
    {
        public int ResolveChunkSize(long fileSize)
        {
            const int KB = 1024;
            const int MB = 1024 * KB;

            return fileSize switch
            {
                < 10 * MB => 512 * KB,
                < 1L * 1024 * MB => 5 * MB,
                _ => 20 * MB
            };
        }
    }
}
