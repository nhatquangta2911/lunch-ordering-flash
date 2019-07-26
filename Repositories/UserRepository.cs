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
            _context.AddCommand(() => DbSet.InsertOneAsync(user));
            var response = await DbSet.Find(x => x.Username == userIn.Username).FirstOrDefaultAsync();
            var token = GeneratingToken.GenerateToken(_appSettings.Secret, response);
            return token;
        }
   }
}