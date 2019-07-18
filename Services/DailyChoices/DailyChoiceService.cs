using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Models;
using CourseApi.Services.DailyChoices.Dtos;
using MongoDB.Driver;

namespace CourseApi.Services.DailyChoices
{
    public class DailyChoiceService
    {
        private readonly IMongoCollection<DailyChoice> _dailyChoices;
        private readonly IMapper _mapper;

        public DailyChoiceService(IDailyChoicestoreDatabaseSettings settings, IMapper mapper)
        {
            _mapper = mapper;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _dailyChoices = database.GetCollection<DailyChoice>(settings.DailyChoicesCollectionName);
        } 

        public async Task<List<DailyChoice>> Get() =>
            await _dailyChoices.Find(dailyChoice => true).ToListAsync();

        public async Task<DailyChoice> Get(string id) =>
            await _dailyChoices.Find<DailyChoice>(dailyChoice => dailyChoice.Id == id).FirstOrDefaultAsync();

        public async Task<DailyChoiceForAddingDto> Create(DailyChoiceForAddingDto dailyChoiceIn)
        {
            var dailyChoice = _mapper.Map<DailyChoice>(dailyChoiceIn);
            await _dailyChoices.InsertOneAsync(dailyChoice);
            return dailyChoiceIn;
        }
        
        public async Task Update(string id, DailyChoice dailyChoiceIn)
        {
            await _dailyChoices.ReplaceOneAsync(dailyChoice => dailyChoice.Id == id, dailyChoiceIn);
        }

        public async Task Delete(string id)
        {
            await _dailyChoices.DeleteOneAsync(dailyChoice => dailyChoice.Id == id);
        }
    }
}