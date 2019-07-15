using System.Collections.Generic;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Models;
using CourseApi.Services.Users.Dtos;
using MongoDB.Driver;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CourseApi.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

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

        public User Authenticate(string username, string password)
        {
            var user = _users.Find(x => x.Username == username).FirstOrDefault();
            
            if (user == null)
                return null;

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (isValidPassword == false)
                return null;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescription = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name", user.Name.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;
            return user;
        }

        public List<User> Get() => 
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User Create(UserRegisterDto user)
        {
            var userEntity = _mapper.Map<User>(user);
            userEntity.Token = GeneratingToken.GenerateToken(_appSettings.Secret.ToString(), userEntity);

            _users.InsertOne(userEntity);
            
            return userEntity;
        }

        public void Update(string id, User userIn)
        {
            _users.ReplaceOne(user => user.Id == id, userIn);
        }

        public void Delete(string id)
        {
            _users.DeleteOne(user => user.Id == id);
        }
    }
}