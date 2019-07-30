using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Interfaces;
using MongoDB.Driver;

namespace CourseApi.Repositories
{
    public class DailyChoiceRepository : ProductionRepository<DailyChoice>, IDailyChoiceRepository
    {
        private readonly IProductionMongoContext _context;
        private readonly IMongoCollection<DailyChoice> DbSet;
        private readonly IMapper _mapper;

        public DailyChoiceRepository(IProductionMongoContext context, IMapper mapper)
            : base(context)
        {
            _context = context;
            _mapper = mapper;
            DbSet = _context.GetCollection<DailyChoice>(typeof(DailyChoice).Name);
        }

        public async Task<DailyChoice> GetToday()
        {
            return await DbSet.Find(_ => true).SortByDescending(_ => _.dateCreated).FirstAsync();
        }
   }
}