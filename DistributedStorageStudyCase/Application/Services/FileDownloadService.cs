using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedStorageStudyCase.Application.Interfaces;
using DistributedStorageStudyCase.Domain.Exceptions;
using DistributedStorageStudyCase.Infrastructure.Hashing;
using DistributedStorageStudyCase.Infrastructure.Metadata;
using DistributedStorageStudyCase.Infrastructure.Storage;
using Microsoft.Extensions.Logging;

namespace DistributedStorageStudyCase.Application.Services
{
    public class FileDownloadService : IFileDownloadService
    {
        private readonly IEnumerable<IStorageProvider> _storageProviders;
        private readonly IMetadataRepository _metadataRepository;
        private readonly IHashService _hashService;
        private readonly ILogger<FileDownloadService> _logger;

        public FileDownloadService(
            IEnumerable<IStorageProvider> storageProviders,
            IMetadataRepository metadataRepository,
            IHashService hashService,
            ILogger<FileDownloadService> logger)
        {
            _storageProviders = storageProviders;
            _metadataRepository = metadataRepository;
            _hashService = hashService;
            _logger = logger;
        }

        public async Task DownloadAsync(Guid fileId, string targetPath)
        {
            _logger.LogInformation("FileId si için indirme başladı. {FileId}", fileId);

            var metadata = await _metadataRepository.GetAsync(fileId);
            using (var output = new FileStream(
                targetPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                foreach (var chunk in metadata.Chunks.OrderBy(c => c.Order))
                {
                    var provider = _storageProviders
                        .First(p => p.Name == chunk.StorageProvider);

                    var data = await provider.ReadAsync(chunk.ChunkKey);
                    output.Write(data);
                }
            } 

            var reconstructedHash = _hashService.ComputeFile(targetPath);

            if (reconstructedHash != metadata.Checksum)
                throw new IntegrityException("Dosya bütünlük hatası.");

            _logger.LogInformation("Dosya indirildi.");
        }
    }
}
