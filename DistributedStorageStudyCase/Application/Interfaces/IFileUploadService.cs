using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedStorageStudyCase.Application.Interfaces
{
    public interface IFileUploadService
    {
        Task<IReadOnlyList<Guid>> UploadAsync(IEnumerable<string> filePaths);
    }
}
