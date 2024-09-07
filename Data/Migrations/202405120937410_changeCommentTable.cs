namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeCommentTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Posts_PostID", "dbo.CmtPosts");
            DropForeignKey("dbo.Comments", "Users_UserID", "dbo.CmtUser");
            DropForeignKey("dbo.SubComments", "Comment_ComID", "dbo.Comments");
            DropForeignKey("dbo.SubComments", "User_UserID", "dbo.CmtUser");
            DropIndex("dbo.Comments", new[] { "Posts_PostID" });
            DropIndex("dbo.Comments", new[] { "Users_UserID" });
            DropIndex("dbo.SubComments", new[] { "Comment_ComID" });
            DropIndex("dbo.SubComments", new[] { "User_UserID" });
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.User_Id)
                .Index(t => t.Product_ID)
                .Index(t => t.User_Id);
            
            DropTable("dbo.CmtPosts");
            DropTable("dbo.Comments");
            DropTable("dbo.CmtUser");
            DropTable("dbo.SubComments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubComments",
                c => new
                    {
                        SubComID = c.Int(nullable: false, identity: true),
                        CommentMsg = c.String(nullable: false, maxLength: 250),
                        CommentedDate = c.DateTime(nullable: false),
                        Comment_ComID = c.Int(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.SubComID);
            
            CreateTable(
                "dbo.CmtUser",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        imageProfile = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ComID = c.Int(nullable: false, identity: true),
                        CommentMsg = c.String(nullable: false, maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        Posts_PostID = c.Int(),
                        Users_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.ComID);
            
            CreateTable(
                "dbo.CmtPosts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 250),
                        PostedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PostID);
            
            DropForeignKey("dbo.Comment", "User_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Comment", "Product_ID", "dbo.Products");
            DropIndex("dbo.Comment", new[] { "User_Id" });
            DropIndex("dbo.Comment", new[] { "Product_ID" });
            DropTable("dbo.Comment");
            CreateIndex("dbo.SubComments", "User_UserID");
            CreateIndex("dbo.SubComments", "Comment_ComID");
            CreateIndex("dbo.Comments", "Users_UserID");
            CreateIndex("dbo.Comments", "Posts_PostID");
            AddForeignKey("dbo.SubComments", "User_UserID", "dbo.CmtUser", "UserID");
            AddForeignKey("dbo.SubComments", "Comment_ComID", "dbo.Comments", "ComID");
            AddForeignKey("dbo.Comments", "Users_UserID", "dbo.CmtUser", "UserID");
            AddForeignKey("dbo.Comments", "Posts_PostID", "dbo.CmtPosts", "PostID");
        }
    }
}
