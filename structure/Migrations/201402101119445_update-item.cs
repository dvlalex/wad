namespace structure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HtmlItem", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HtmlItem", "Type");
        }
    }
}
