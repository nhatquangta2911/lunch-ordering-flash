using System.Collections.Generic;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Models;
using CourseApi.Services.Users.Dtos;
using MongoDB.Driver;

namespace CourseApi.Services.Users
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMapper _mapper;

        public UserService(IUserstoreDatabaseSettings settings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> Get() => 
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User Create(UserDto user)
        {
            var userEntity = _mapper.Map<User>(user);
            _users.InsertOne(userEntity);
            return userEntity;
        }
    }
}