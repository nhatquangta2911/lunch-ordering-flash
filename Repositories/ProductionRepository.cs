using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApi.Interfaces;
using MongoDB.Driver;
using ServiceStack;

namespace CourseApi.Repositories
{
    public class ProductionRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IProductionMongoContext _context;
        private readonly IMongoCollection<TEntity> DbSet;

        public ProductionRepository(IProductionMongoContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Add(TEntity obj)
        {
            _context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("Id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj)
        {
            _context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("Id", obj.GetId()), obj));
        }

        public virtual void Remove(string id)
        {
            _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("Id", id)));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
   }
}