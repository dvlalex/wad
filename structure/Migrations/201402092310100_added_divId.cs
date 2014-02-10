namespace structure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_divId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HtmlSnippet", "DivId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HtmlSnippet", "DivId");
        }
    }
}
