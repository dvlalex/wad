namespace structure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userId_to_int : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HtmlSnippet", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HtmlSnippet", "UserId", c => c.String());
        }
    }
}
