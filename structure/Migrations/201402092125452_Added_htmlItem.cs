namespace structure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_htmlItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HtmlItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDateTime = c.DateTime(nullable: false),
                        user_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserSession", t => t.user_UserId)
                .Index(t => t.user_UserId);
            
            AddColumn("dbo.HtmlSnippet", "Item_Id", c => c.Int());
            AddColumn("dbo.HtmlSnippet", "User_UserId", c => c.Int());
            CreateIndex("dbo.HtmlSnippet", "Item_Id");
            CreateIndex("dbo.HtmlSnippet", "User_UserId");
            AddForeignKey("dbo.HtmlSnippet", "Item_Id", "dbo.HtmlItem", "Id");
            AddForeignKey("dbo.HtmlSnippet", "User_UserId", "dbo.UserSession", "UserId");
            DropColumn("dbo.HtmlSnippet", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HtmlSnippet", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.HtmlSnippet", "User_UserId", "dbo.UserSession");
            DropForeignKey("dbo.HtmlSnippet", "Item_Id", "dbo.HtmlItem");
            DropForeignKey("dbo.HtmlItem", "user_UserId", "dbo.UserSession");
            DropIndex("dbo.HtmlSnippet", new[] { "User_UserId" });
            DropIndex("dbo.HtmlSnippet", new[] { "Item_Id" });
            DropIndex("dbo.HtmlItem", new[] { "user_UserId" });
            DropColumn("dbo.HtmlSnippet", "User_UserId");
            DropColumn("dbo.HtmlSnippet", "Item_Id");
            DropTable("dbo.HtmlItem");
        }
    }
}
