using System;
using MongoDB.Driver;

namespace CourseApi.Interfaces
{
    public interface IProductionMongoContext : IMongoContext, IDisposable
    {
         IMongoCollection<T> GetCollection<T>(string name);
    }
}