namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeId_addFullname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "FullName", c => c.String());
            AlterColumn("dbo.Comments", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "UserID", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "FullName");
        }
    }
}
