using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CourseApi.Interfaces
{
    public interface IMongoContext : IDisposable
    {
         void AddCommand(Func<Task> func);
         Task<int> SaveChanges();
    }
}