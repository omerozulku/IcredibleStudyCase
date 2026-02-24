using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedStorageStudyCase.Domain.Entities;
using DistributedStorageStudyCase.Persistence.Context;
using Microsoft.EntityFrameworkCore;
namespace DistributedStorageStudyCase.Infrastructure.Metadata
{
    public class MetadataRepository : IMetadataRepository
    {
        private readonly MetadataDbContext _context;

        public MetadataRepository(MetadataDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(FileMetadata metadata)
        {
            _context.Files.Add(metadata);
            await _context.SaveChangesAsync();
        }

        public async Task<FileMetadata> GetAsync(Guid fileId)
        {
            return await _context.Files
                .Include(f => f.Chunks)
                .FirstAsync(f => f.FileId == fileId);
        }
        public async Task<IReadOnlyList<FileMetadata>> GetAllAsync()
        {
            return await _context.Files
                .AsNoTracking()
                .Include(f => f.Chunks)
                .OrderByDescending(f => f.FileId)
                .ToListAsync();
        }
    }
}
