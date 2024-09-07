namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delUseeName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Comments", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "FullName", c => c.String());
        }
    }
}
