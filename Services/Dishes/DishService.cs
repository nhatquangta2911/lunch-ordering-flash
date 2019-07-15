using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Models;
using CourseApi.Services.Dishes.Dtos;
using MongoDB.Driver;

namespace CourseApi.Services.Dishes
{
    public class DishService
    {
        private readonly IMongoCollection<Dish> _dishes;
        private readonly IMapper _mapper;

        public DishService(IDishstoreDatabaseSettings settings, IMapper mapper)
        {
            _mapper = mapper;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _dishes = database.GetCollection<Dish>(settings.DishesCollectionName);
        }

        public async Task<List<Dish>> Get() =>
            await _dishes.Find(user => true).ToListAsync();

        public async Task<Dish> Get(string id) =>
            await _dishes.Find<Dish>(dish => dish.Id == id).FirstOrDefaultAsync();

        public async Task<Dish> Create(DishForAddingDto dishForAdding)
        {
            var dish = _mapper.Map<DishForAddingDto, Dish>(dishForAdding);
            await _dishes.InsertOneAsync(dish);
            return dish;
        }

        public async Task Update(string id, Dish dishIn)
        {
            await _dishes.ReplaceOneAsync(dish => dish.Id == id, dishIn);
        }

        public async Task Delete(string id)
        {
            await _dishes.DeleteOneAsync(dish => dish.Id == id);
        }

    }
}