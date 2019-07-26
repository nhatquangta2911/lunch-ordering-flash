using CourseApi.Entities;
using CourseApi.Interfaces;

namespace CourseApi.Repositories
{
    public class DailyChoiceRepository : ProductionRepository<DailyChoice>, IDailyChoiceRepository
    {
        public DailyChoiceRepository(IProductionMongoContext context)
            : base(context)
        {
        }
    }
}