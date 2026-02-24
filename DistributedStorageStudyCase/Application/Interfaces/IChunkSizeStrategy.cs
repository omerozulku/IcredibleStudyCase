using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedStorageStudyCase.Application.Interfaces
{
    public interface IChunkSizeStrategy
    {
        int ResolveChunkSize(long fileSize);
    }
}
