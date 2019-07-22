using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseApi.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class 
    {
         void Add(TEntity obj);
         Task<TEntity> GetById(string id);
         Task<IEnumerable<TEntity>> GetAll();
         void Update(TEntity obj);
         void Remove(string id);
    }
}