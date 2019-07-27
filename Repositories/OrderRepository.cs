using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Interfaces;
using MongoDB.Driver;

namespace CourseApi.Repositories
{
    public class OrderRepository : ProductionRepository<Order>, IOrderRepository
    {
        private readonly IProductionMongoContext _context;
        private readonly IMongoCollection<Order> DbSet;
        private readonly IMapper _mapper;

        public OrderRepository(IProductionMongoContext context, IMapper mapper)
            : base(context)
        {
            _context = context;
            _mapper = mapper;
            DbSet = _context.GetCollection<Order>(typeof(Order).Name);
        }

        public async Task<List<Order>> GetOrdersByUser(string id)
        {
            return await DbSet.Find(_ => _.UserId == id).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByDailyChoice(string id)
        {
            return await DbSet.Find(_ => _.DailyChoiceId == id).ToListAsync();
        }

        public bool IsOverdue(DateTime dateCreated, double validAmountOfTime)
        {
            var now = DateTime.UtcNow;
            return (now - dateCreated).TotalHours >= validAmountOfTime;
        }

        public bool IsOrdered(List<Order> ordersByUser, string dailyChoiceId)
        {
            return ordersByUser.Find(_ => _.DailyChoiceId == dailyChoiceId) != null;
        }
   }
}