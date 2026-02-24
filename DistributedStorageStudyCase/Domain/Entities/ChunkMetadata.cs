using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedStorageStudyCase.Domain.Entities
{
    public class ChunkMetadata
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ChunkKey { get; set; }
        public int Order { get; set; }
        public string StorageProvider { get; set; }
        public string Checksum { get; set; }
    }
}
