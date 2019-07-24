using System;
using CourseApi.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CourseApi.Context
{
    public class MockMongoContext : MongoContext, IMockMongoContext
    {
        private IMongoDatabase Database { get; set; }

        public MockMongoContext(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("MOCKMONGOCONNECTION") ?? configuration.GetSection("MockMongoSettings").GetSection("Connection").Value);
            Database = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("MOCKDATABASENAME") ?? configuration.GetSection("MockMongoSettings").GetSection("DatabaseName").Value);
        }   

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }
    }
}