using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Entities;
using CourseApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Repositories
{
    public class PhotoRepository : IphotoRepository
    {
        private readonly CourseApiDbContext _context;
        public PhotoRepository(CourseApiDbContext context)
        {
            _context = context;
        }

        public async Task<Photo> GetPhoto(string dishId)
        {
            return await _context.Photos.SingleAsync();
        }
   }
}