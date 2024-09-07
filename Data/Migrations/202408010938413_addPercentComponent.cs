namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPercentComponent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PercentComponents", "Component", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PercentComponents", "Component", c => c.String(maxLength: 250));
        }
    }
}
