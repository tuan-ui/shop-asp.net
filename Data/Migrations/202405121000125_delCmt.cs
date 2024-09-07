namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delCmt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comment", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.Comment", "User_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.Comment", new[] { "Product_ID" });
            DropIndex("dbo.Comment", new[] { "User_Id" });
            DropTable("dbo.Comment");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CommentMsg = c.String(),
                        CommentDate = c.DateTime(nullable: false),
                        ParentID = c.Int(nullable: false),
                        Rate = c.Int(nullable: false),
                        Product_ID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Comment", "User_Id");
            CreateIndex("dbo.Comment", "Product_ID");
            AddForeignKey("dbo.Comment", "User_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.Comment", "Product_ID", "dbo.Products", "ID");
        }
    }
}
