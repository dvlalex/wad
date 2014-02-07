
using System.Data.Entity;

namespace structure.sql
{
    public class WadDatabaseSetup : DropCreateDatabaseIfModelChanges<WadDbContext>
    {
        protected override void Seed(WadDbContext context)
        {
            base.Seed(context);
            //here rest
        }
    }
}
