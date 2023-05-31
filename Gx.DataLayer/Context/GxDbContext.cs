using System.Linq;
using Gx.DataLayer.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Gx.DataLayer.Entities.Queue;

namespace Gx.DataLayer.Context
{
    public class GxDbContext : DbContext
    {
        #region constructor

        public GxDbContext(DbContextOptions<GxDbContext> options) : base(options)
        {
        }

        #endregion

        #region Db Sets

        public DbSet<Users> Users { get; set; }

        //public DbSet<Plan> Plan { get; set; }
        public DbSet<Queue> Queue { get; set; }

        #endregion

        #region disable cascading delete in database

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascades = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascades)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}