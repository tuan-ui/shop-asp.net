namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSearch2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductSearchs",
                c => new
                    {
                        CategoryID = c.Int(nullable: false),
                        SearchID = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.CategoryID, t.SearchID })
                .ForeignKey("dbo.ProductCategories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Searchs", t => t.SearchID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.SearchID);
            
            CreateTable(
                "dbo.Searchs",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductSearchs", "SearchID", "dbo.Searchs");
            DropForeignKey("dbo.ProductSearchs", "CategoryID", "dbo.ProductCategories");
            DropIndex("dbo.ProductSearchs", new[] { "SearchID" });
            DropIndex("dbo.ProductSearchs", new[] { "CategoryID" });
            DropTable("dbo.Searchs");
            DropTable("dbo.ProductSearchs");
        }
    }
}
