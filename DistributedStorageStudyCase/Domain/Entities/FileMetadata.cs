using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedStorageStudyCase.Domain.Entities
{
    public class FileMetadata
    {
        public Guid FileId { get; set; }
        public string OriginalFileName { get; set; }
        public string Checksum { get; set; }

        public ICollection<ChunkMetadata> Chunks { get; set; } = new List<ChunkMetadata>();
    }
}
