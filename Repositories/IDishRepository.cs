using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApi.Entities;

namespace CourseApi.Repositories
{
    public interface IDishRepository
    {
         Task<Dish> GetDish(string id, bool includeRelated = true);
         void Add(Dish dish);
         void Remove(Dish dish);
         Task<IEnumerable<Dish>> GetDishes();
    }
}