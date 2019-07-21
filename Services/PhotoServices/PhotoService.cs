using System.Threading.Tasks;
using CourseApi.Entities;
using CourseApi.Helpers;
using Microsoft.AspNetCore.Http;

namespace CourseApi.Services.PhotoServices
{
   public class PhotoService : IPhotoService
   {
       private readonly IUnitOfWork _unitOfWork;
       private readonly IPhotoStorage _photoStorage;
       public PhotoService(IUnitOfWork unitOfWork, IPhotoStorage photoStorage)
        {
            this._unitOfWork = unitOfWork;
            this._photoStorage = photoStorage;
        }
       
       public async Task<Photo> UploadPhoto(Dish dish, IFormFile file, string uploadsFolderPath)
       {
           var fileName = await _photoStorage.StorePhoto(uploadsFolderPath, file);
           var photo = new Photo { FileName = fileName };
           dish.Photo = photo;
           await _unitOfWork.CompleteAsync();

           return photo; 
       }
   }
}