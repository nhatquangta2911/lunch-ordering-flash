using System;
using MongoDB.Driver;

namespace CourseApi.Interfaces
{
    public interface IMockMongoContext : IMongoContext
    {
         IMongoCollection<T> GetCollection<T>(string name);
    }
}