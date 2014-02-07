
using System.Data.Entity;
using data.entity;

namespace structure.sql
{
    public class WadDbContext : DbContext
    {
        public WadDbContext()
            : base("WadDatabase")
        {
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //model table bindings
            modelBuilder.Entity<UserSession>().ToTable("UserSession");
        }
    }

}
