namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCmt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CmtPosts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 250),
                        PostedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PostID);
            
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
                .PrimaryKey(t => t.ComID)
                .ForeignKey("dbo.CmtPosts", t => t.Posts_PostID)
                .ForeignKey("dbo.CmtUser", t => t.Users_UserID)
                .Index(t => t.Posts_PostID)
                .Index(t => t.Users_UserID);
            
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
                "dbo.SubComments",
                c => new
                    {
                        SubComID = c.Int(nullable: false, identity: true),
                        CommentMsg = c.String(nullable: false, maxLength: 250),
                        CommentedDate = c.DateTime(nullable: false),
                        Comment_ComID = c.Int(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.SubComID)
                .ForeignKey("dbo.Comments", t => t.Comment_ComID)
                .ForeignKey("dbo.CmtUser", t => t.User_UserID)
                .Index(t => t.Comment_ComID)
                .Index(t => t.User_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubComments", "User_UserID", "dbo.CmtUser");
            DropForeignKey("dbo.SubComments", "Comment_ComID", "dbo.Comments");
            DropForeignKey("dbo.Comments", "Users_UserID", "dbo.CmtUser");
            DropForeignKey("dbo.Comments", "Posts_PostID", "dbo.CmtPosts");
            DropIndex("dbo.SubComments", new[] { "User_UserID" });
            DropIndex("dbo.SubComments", new[] { "Comment_ComID" });
            DropIndex("dbo.Comments", new[] { "Users_UserID" });
            DropIndex("dbo.Comments", new[] { "Posts_PostID" });
            DropTable("dbo.SubComments");
            DropTable("dbo.CmtUser");
            DropTable("dbo.Comments");
            DropTable("dbo.CmtPosts");
        }
    }
}
