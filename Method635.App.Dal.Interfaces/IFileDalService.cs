using System.IO;
using System.Threading.Tasks;

namespace Method635.App.Dal.Interfaces
{
    public interface IFileDalService
    {
        string UploadFile(Stream stream);
        Task<Stream> Download(string fileId);
    }
}
