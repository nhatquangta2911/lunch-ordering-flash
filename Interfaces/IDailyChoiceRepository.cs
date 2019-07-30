using System.Threading.Tasks;
using CourseApi.Entities;
using CourseApi.Repositories;

namespace CourseApi.Interfaces
{
    public interface IDailyChoiceRepository : IRepository<DailyChoice>
    {
         Task<DailyChoice> GetToday();
    }
}