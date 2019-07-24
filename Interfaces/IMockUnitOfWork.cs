using System;
using System.Threading.Tasks;

namespace CourseApi.Interfaces
{
    public interface IMockUnitOfWork : IDisposable
    {
         Task<bool> Commit();
    } 
}