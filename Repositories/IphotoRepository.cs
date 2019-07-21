using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApi.Entities;

namespace CourseApi.Repositories
{
    public interface IphotoRepository
    {
         Task<Photo> GetPhoto(string dishId);
    }
}