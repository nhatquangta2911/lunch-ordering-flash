using System;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Helpers;
using CourseApi.Interfaces;
using CourseApi.Services.Users.Dtos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CourseApi.Repositories
{
    public class UserRepository : ProductionRepository<User>, IUserRepository
    {

        private readonly IProductionMongoContext _context;
        private readonly IMongoCollection<User> DbSet;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserRepository(IProductionMongoContext context, IMapper mapper, IOptions<AppSettings> appSettings)
            : base(context)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            DbSet = _context.GetCollection<User>(typeof(User).Name);
        }

        public async Task<string> CreateAsync(UserRegisterDto userIn)
        {
            var user = _mapper.Map<User>(userIn);
            await DbSet.InsertOneAsync(user);
            var response = await DbSet.Find(_ => _.Id == user.Id).FirstOrDefaultAsync();
            var token = GeneratingToken.GenerateToken(_appSettings.Secret, response);
            return token;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await DbSet.Find(_ => _.Username == username).FirstOrDefaultAsync();
            if(user == null)
                return null;
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if(!isValidPassword) 
                return null;
            
            var token = GeneratingToken.GenerateToken(_appSettings.Secret, user);
            return token;
        }
   }
}