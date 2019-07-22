using CourseApi.Entities;
using CourseApi.Interfaces;

namespace CourseApi.Repositories
{
    public class DishRepository : BaseRepository<Dish>, IDishRepository
    {
        public DishRepository(IMongoContext context)
            : base (context)
        {
        }
   }
}