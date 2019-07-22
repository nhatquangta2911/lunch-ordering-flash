using System;
using System.Threading.Tasks;

namespace CourseApi.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
         Task<bool> Commit();
    }
}