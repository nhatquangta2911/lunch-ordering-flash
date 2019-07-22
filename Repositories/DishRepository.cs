using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApi.Entities;
using CourseApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly CourseApiDbContext _context;

        public DishRepository(CourseApiDbContext context)
        {
            _context = context;
        }

      public void Add(Dish dish)
      {
         throw new System.NotImplementedException();
      }

      public async Task<Dish> GetDish(string id, bool includeRelated = true)
      {
         return await _context.Dishes.FindAsync(id);
      }

      public Task<IEnumerable<Dish>> GetDishes()
      {
         throw new System.NotImplementedException();
      }

      public void Remove(Dish dish)
      {
         throw new System.NotImplementedException();
      }
   }
}