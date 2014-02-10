namespace structure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated_htmpsnippet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HtmlSnippet", "Item_Id", "dbo.HtmlItem");
            DropForeignKey("dbo.HtmlSnippet", "User_UserId", "dbo.UserSession");
            DropForeignKey("dbo.HtmlItem", "user_UserId", "dbo.UserSession");
            DropIndex("dbo.HtmlSnippet", new[] { "Item_Id" });
            DropIndex("dbo.HtmlSnippet", new[] { "User_UserId" });
            DropIndex("dbo.HtmlItem", new[] { "user_UserId" });
            AddColumn("dbo.HtmlSnippet", "HtmlItem_Id", c => c.Int());
            AlterColumn("dbo.HtmlItem", "User_UserId", c => c.Int());
            CreateIndex("dbo.HtmlSnippet", "HtmlItem_Id");
            CreateIndex("dbo.HtmlItem", "User_UserId");
            AddForeignKey("dbo.HtmlSnippet", "HtmlItem_Id", "dbo.HtmlItem", "Id");
            AddForeignKey("dbo.HtmlItem", "User_UserId", "dbo.UserSession", "UserId");
            DropColumn("dbo.HtmlSnippet", "RdfCode");
            DropColumn("dbo.HtmlSnippet", "Item_Id");
            DropColumn("dbo.HtmlSnippet", "User_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HtmlSnippet", "User_UserId", c => c.Int());
            AddColumn("dbo.HtmlSnippet", "Item_Id", c => c.Int());
            AddColumn("dbo.HtmlSnippet", "RdfCode", c => c.String());
            DropForeignKey("dbo.HtmlItem", "User_UserId", "dbo.UserSession");
            DropForeignKey("dbo.HtmlSnippet", "HtmlItem_Id", "dbo.HtmlItem");
            DropIndex("dbo.HtmlItem", new[] { "User_UserId" });
            DropIndex("dbo.HtmlSnippet", new[] { "HtmlItem_Id" });
            AlterColumn("dbo.HtmlItem", "User_UserId", c => c.Int());
            DropColumn("dbo.HtmlSnippet", "HtmlItem_Id");
            CreateIndex("dbo.HtmlItem", "user_UserId");
            CreateIndex("dbo.HtmlSnippet", "User_UserId");
            CreateIndex("dbo.HtmlSnippet", "Item_Id");
            AddForeignKey("dbo.HtmlItem", "user_UserId", "dbo.UserSession", "UserId");
            AddForeignKey("dbo.HtmlSnippet", "User_UserId", "dbo.UserSession", "UserId");
            AddForeignKey("dbo.HtmlSnippet", "Item_Id", "dbo.HtmlItem", "Id");
        }
    }
}
