namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedAttribute : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Comments", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Comments", "ProductID", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "UserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "ParentID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Comments", "ID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Comments", "ParentID", c => c.Long(nullable: false));
            AlterColumn("dbo.Comments", "UserID", c => c.Long(nullable: false));
            AlterColumn("dbo.Comments", "ProductID", c => c.Long(nullable: false));
            AlterColumn("dbo.Comments", "ID", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Comments", "ID");
        }
    }
}
