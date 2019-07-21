using System.Threading.Tasks;

namespace CourseApi.Helpers
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}