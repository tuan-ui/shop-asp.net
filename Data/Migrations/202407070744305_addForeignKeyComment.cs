namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForeignKeyComment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "UserID");
            AddForeignKey("dbo.Comments", "UserID", "dbo.ApplicationUsers", "Id");
            DropColumn("dbo.Comments", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "FullName", c => c.String());
            DropForeignKey("dbo.Comments", "UserID", "dbo.ApplicationUsers");
            DropIndex("dbo.Comments", new[] { "UserID" });
            AlterColumn("dbo.Comments", "UserID", c => c.String());
        }
    }
}
