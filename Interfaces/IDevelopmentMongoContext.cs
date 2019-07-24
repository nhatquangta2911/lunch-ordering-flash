using System;
using MongoDB.Driver;

namespace CourseApi.Interfaces
{
    public interface IDevelopmentMongoContext : IMongoContext, IDisposable
    {
         IMongoCollection<T> GetCollection<T>(string name);
    }
}