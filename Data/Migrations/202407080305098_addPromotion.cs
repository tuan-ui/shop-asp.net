namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPromotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Promotions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 256),
                        Name = c.String(nullable: false, maxLength: 256),
                        Quantity = c.Int(nullable: false),
                        ProductID = c.Int(),
                        DiscountPercent = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promotions", "ProductID", "dbo.Products");
            DropIndex("dbo.Promotions", new[] { "ProductID" });
            DropTable("dbo.Promotions");
        }
    }
}
