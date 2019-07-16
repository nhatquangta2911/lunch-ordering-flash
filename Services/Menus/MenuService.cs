using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Models;
using MongoDB.Driver;

namespace CourseApi.Services.Menus
{
    public class MenuService
    {
        private readonly IMongoCollection<Menu> _menus;
        private readonly IMapper _mapper;

        public MenuService(IMenustoreDatabaseSettings settings, IMapper mapper)
        {
            _mapper = mapper;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _menus = database.GetCollection<Menu>(settings.MenusCollectionName);
        }

        public async Task<List<Menu>> Get() => 
            await _menus.Find(menu => true).ToListAsync();

        public async Task<Menu> Get(string id) =>
            await _menus.Find<Menu>(menu => menu.Id == id).FirstOrDefaultAsync();

        public async Task<Menu> Create(Menu menu)
        {
            await _menus.InsertOneAsync(menu);
            return menu;
        }

        public async Task Update(string id, Menu menuIn)
        {
            await _menus.ReplaceOneAsync(menu => menu.Id == id, menuIn);
        }

        public async Task Delete(string id)
        {
            await _menus.DeleteOneAsync(menu => menu.Id == id);
        }

    }
}