namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWarranty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Warranty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Warranty");
        }
    }
}
