using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Admins.Persistance
{
    public class AdminsRepository : IAdminsRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public AdminsRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Admin?> GetByIdAsync(Guid adminId)
        {
            return _dbContext.Admins.FirstOrDefaultAsync(a => a.Id == adminId);
        }

        public Task UpdateAsync(Admin admin)
        {
            _dbContext.Admins.Update(admin);
            return Task.CompletedTask;
        }
    }
}