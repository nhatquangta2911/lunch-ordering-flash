using System.Collections.Generic;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Models;
using MongoDB.Driver;
using System.Linq;
using CourseApi.Helpers;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using CourseApi.Services.Users.Dtos;

namespace CourseApi.Services.Users
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

      public UserService(IUserstoreDatabaseSettings settings, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);

            _users.AsQueryable<User>()
                  .Select(c => c.Username)
                  .Distinct();

            _appSettings = appSettings.Value;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _users.Find(x => x.Username == username).FirstOrDefaultAsync();
            if (user == null)
                return null;
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);   
            if (isValidPassword == false)
                return null;
        
            var token = GeneratingToken.GenerateToken(_appSettings.Secret, user);
            return token;
        }

        public async Task<List<User>> Get() =>
            await _users.Find(user => true).ToListAsync();

        public async Task<UserResponseDto> Get(string id) {
            var userOut = await _users.Find<User>(user => user.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<UserResponseDto>(userOut);
        }
        public async Task<string> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            var response = await _users.Find(x => x.Id == user.Id).FirstOrDefaultAsync();
            var token = GeneratingToken.GenerateToken(_appSettings.Secret, user);
            
            return token;
        }

        public async Task UpdateAsync(string id, User userIn)
        {
            await _users.ReplaceOneAsync(user => user.Id == id, userIn);
        }

        public async Task DeleteAsync(string id)
        {
            await _users.DeleteOneAsync(user => user.Id == id);
        }
    }
}