using System;
using CourseApi.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CourseApi.Context
{
    public class ProductionMongoContext : MongoContext, IProductionMongoContext
    {
        private IMongoDatabase Database { get; set; }

        public ProductionMongoContext(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("MONGOCONNECTION") ?? configuration.GetSection("MongoSettings").GetSection("Connection").Value);
            Database = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DATABASENAME") ?? configuration.GetSection("MongoSettings").GetSection("DatabaseName").Value);
        }   

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }
    }
}