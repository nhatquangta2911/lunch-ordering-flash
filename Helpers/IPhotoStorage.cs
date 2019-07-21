using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CourseApi.Helpers
{
    public interface IPhotoStorage
    {
         Task<string> StorePhoto(string uploadsFolderPath, IFormFile file);
    }
}