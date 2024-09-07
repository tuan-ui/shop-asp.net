namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSupplierIdToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SupplierID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "SupplierID");
        }
    }
}
