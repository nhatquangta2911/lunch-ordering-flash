using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApi.Entities;
using CourseApi.Repositories;

namespace CourseApi.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetOrdersByUser(string userId);
        Task<List<Order>> GetOrdersByDailyChoice(string dailyChoiceId);
        bool IsOverdue(DateTime dateCreated, double validAmountOfTime);
        bool IsOrdered(List<Order> ordersByUser, string dailyChoiceId);
    }
}