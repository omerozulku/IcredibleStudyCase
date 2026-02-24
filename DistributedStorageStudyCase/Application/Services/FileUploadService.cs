using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedStorageStudyCase.Application.Interfaces;
using DistributedStorageStudyCase.Domain.Entities;
using DistributedStorageStudyCase.Infrastructure.Chunking;
using DistributedStorageStudyCase.Infrastructure.Hashing;
using DistributedStorageStudyCase.Infrastructure.Metadata;
using DistributedStorageStudyCase.Infrastructure.Storage;
using Microsoft.Extensions.Logging;

namespace DistributedStorageStudyCase.Application.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IEnumerable<IStorageProvider> _storageProviders;
        private readonly IChunkSizeStrategy _chunkSizeStrategy;
        private readonly IHashService _hashService;
        private readonly IMetadataRepository _metadataRepository;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(
            IEnumerable<IStorageProvider> storageProviders,
            IChunkSizeStrategy chunkSizeStrategy,
            IHashService hashService,
            IMetadataRepository metadataRepository,
            ILogger<FileUploadService> logger)
        {
            _storageProviders = storageProviders;
            _chunkSizeStrategy = chunkSizeStrategy;
            _hashService = hashService;
            _metadataRepository = metadataRepository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Guid>> UploadAsync(IEnumerable<string> filePaths)
        {
            var uploadedFileIds = new List<Guid>();

            foreach (var path in filePaths)
            {
                var fileId = await UploadSingleFileAsync(path);
                uploadedFileIds.Add(fileId);
            }

            return uploadedFileIds;
        }

        private async Task<Guid> UploadSingleFileAsync(string filePath)
        {
            _logger.LogInformation("{File} dosyası yükleniyor.", filePath);

            var fileInfo = new FileInfo(filePath);
            var chunkSize = _chunkSizeStrategy.ResolveChunkSize(fileInfo.Length);

            var chunker = new Chunker();
            var chunks = chunker.Split(filePath, chunkSize).ToList();

            var fileMetadata = new FileMetadata
            {
                FileId = Guid.NewGuid(),
                OriginalFileName = fileInfo.Name
            };

            var providers = _storageProviders.ToList();

            for (int i = 0; i < chunks.Count; i++)
            {
                var provider = providers[i % providers.Count];
                var chunkKey = $"{fileMetadata.FileId}_{i}";

                await provider.SaveAsync(chunkKey, chunks[i]);

                fileMetadata.Chunks.Add(new ChunkMetadata
                {
                    ChunkKey = chunkKey,
                    Order = i,
                    StorageProvider = provider.Name,
                    Checksum = _hashService.Compute(chunks[i])
                });
            }

            fileMetadata.Checksum = _hashService.ComputeFile(filePath);

            await _metadataRepository.SaveAsync(fileMetadata);

            _logger.LogInformation(
                "{File} dosyası için yükleme tamamlandı. FileId: {FileId}",
                filePath,
                fileMetadata.FileId);

            return fileMetadata.FileId;
        }
    }
}
