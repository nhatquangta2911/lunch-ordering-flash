using System.Threading.Tasks;
using CourseApi.Entities;
using Microsoft.AspNetCore.Http;

namespace CourseApi.Services.PhotoServices
{
    public interface IPhotoService
    {
        Task<Photo> UploadPhoto(Dish dish, IFormFile file, string uploadsFolderPath);
    }
}