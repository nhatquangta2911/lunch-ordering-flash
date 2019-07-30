using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApi.Entities;
using CourseApi.Repositories;
using CourseApi.Services.Users.Dtos;

namespace CourseApi.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
         Task<string> CreateAsync(UserRegisterDto userIn);
         Task<string> AuthenticateAsync(string username, string password);
    }
}