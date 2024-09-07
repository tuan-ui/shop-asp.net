namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFullName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "FullName");
        }
    }
}
