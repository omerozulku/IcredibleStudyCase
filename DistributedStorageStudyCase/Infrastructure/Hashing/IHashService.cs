using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedStorageStudyCase.Infrastructure.Hashing
{
    public interface IHashService
    {
        string Compute(byte[] data);
        string ComputeFile(string path);
    }
}
