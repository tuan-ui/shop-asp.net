namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reAddCmt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CommentMsg = c.String(),
                        CommentDate = c.DateTime(nullable: false),
                        ProductID = c.Long(nullable: false),
                        UserID = c.Long(nullable: false),
                        ParentID = c.Long(nullable: false),
                        Rate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Comments");
        }
    }
}
