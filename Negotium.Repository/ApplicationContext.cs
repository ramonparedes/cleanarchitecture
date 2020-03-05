using Microsoft.EntityFrameworkCore;
using Negotium.Common;
using Negotium.Data;
using System;
using System.Linq;

namespace Negotium.Repository
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public ApplicationContext()
        {

        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserMap(modelBuilder.Entity<User>());
        }
        public override int SaveChanges()
        {
            var baseEntity = ChangeTracker.Entries<AuditableBaseEntity>();

            if(baseEntity != null)
            {
                Guid userId;
                var currentDate = DateTime.UtcNow;

                foreach (var item in baseEntity.Where(w => w.State == EntityState.Added || w.State == EntityState.Modified))
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            //item.Entity.CreatedById = userId;
                            item.Entity.CreatedDate = currentDate;
                            break;
                        case EntityState.Modified:
                            //item.Entity.UpdatedById = userId;
                            item.Entity.UpdatedDate = currentDate;
                            break;
                        default:
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseSqlServer("Data Source=dev.cdevteam.com\\sql2016,57500;Initial Catalog=OADb;User ID=turnospr; Password=turnos");
        }
    }
}
