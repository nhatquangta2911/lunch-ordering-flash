using CourseApi.Entities;
using CourseApi.Interfaces;

namespace CourseApi.Repositories
{
    public class MenuRepository : ProductionRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IProductionMongoContext context)
            : base(context)
        {
        }
    }
}