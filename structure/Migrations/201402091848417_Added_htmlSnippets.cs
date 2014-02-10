namespace structure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_htmlSnippets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HtmlSnippet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        HtmlCode = c.String(),
                        RdfCode = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HtmlSnippet");
        }
    }
}
