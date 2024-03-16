using System.Reflection;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common
{
    public class GymManagementDbContext : DbContext, IUnitOfWork
    {
        public GymManagementDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Subscription> Subscriptions { get; set; } = null!;

        public Task CommitChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

    }
}