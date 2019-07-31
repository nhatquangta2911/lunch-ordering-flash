using CourseApi.Entities;
using CourseApi.Interfaces;

namespace CourseApi.Repositories
{
    public class DishMockRepository : MockRepository<Dish>, IDishMockRepository
    {
        public DishMockRepository(IMockMongoContext context)
            : base(context)
        {
        }
    }
}