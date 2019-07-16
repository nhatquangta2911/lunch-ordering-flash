using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Models;
using MongoDB.Driver;

namespace CourseApi.Services.Orders
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly IMapper _mapper;

        public OrderService(IOrderstoreDatabaseSettings settings, IMapper mapper)
        {
            _mapper = mapper;
            
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _orders = database.GetCollection<Order>(settings.OrdersCollectionName);
        }

        public async Task<List<Order>> Get() => 
            await _orders.Find(menu => true).ToListAsync();

        public async Task<Order> Get(string id) => 
            await _orders.Find<Order>(order => order.Id == id).FirstOrDefaultAsync();

        public async Task<Order> Create(Order order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task Update(string id, Order orderIn)
        {
            await _orders.ReplaceOneAsync(order => order.Id == id, orderIn);
        }

        public async Task Delete(string id)
        {
            await _orders.DeleteOneAsync(order => order.Id == id);
        }

    }
}