using CourseApi.Entities;
using CourseApi.Interfaces;

namespace CourseApi.Repositories
{
    public class DishRepository : ProductionRepository<Dish>, IDishRepository
    {
        public DishRepository(IProductionMongoContext context)
            : base(context)
        {
        }
   }
}