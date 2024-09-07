namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyComment : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Comments", "ProductID");
            AddForeignKey("dbo.Comments", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ProductID", "dbo.Products");
            DropIndex("dbo.Comments", new[] { "ProductID" });
        }
    }
}
