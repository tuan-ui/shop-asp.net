namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSupplierIdForenignKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Products", "SupplierID");
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Supplier", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Supplier");
            DropIndex("dbo.Products", new[] { "SupplierID" });
        }
    }
}
