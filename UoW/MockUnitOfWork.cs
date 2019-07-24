using System.Threading.Tasks;
using CourseApi.Interfaces;

namespace CourseApi.UoW
{
    public class MockUnitOfWork : IMockUnitOfWork
    {
        private readonly IMockMongoContext _context;

        public MockUnitOfWork(IMockMongoContext context)
        {
            _context = context;
        }
        
         public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChanges();
            return changeAmount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}