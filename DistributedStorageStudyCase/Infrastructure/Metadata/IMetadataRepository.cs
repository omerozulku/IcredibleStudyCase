using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedStorageStudyCase.Domain.Entities;

namespace DistributedStorageStudyCase.Infrastructure.Metadata
{
    public interface IMetadataRepository
    {
        Task SaveAsync(FileMetadata metadata);
        Task<FileMetadata> GetAsync(Guid fileId);
        Task<IReadOnlyList<FileMetadata>> GetAllAsync();
    }
}
