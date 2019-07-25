using System.Threading.Tasks;
using CourseApi.Entities;
using CourseApi.Interfaces;
using CourseApi.Services.Users.Dtos;

namespace CourseApi.Repositories
{
    public class UserRepository : ProductionRepository<User>, IUserRepository
    {


        public UserRepository(IProductionMongoContext context)
            : base(context)
        {
        }

        public async Task<string> CreateAsync(UserRegisterDto userIn)
        {
            // var user = _mapper.Map<User>(userIn);
            return "temp";
        }
    }
}