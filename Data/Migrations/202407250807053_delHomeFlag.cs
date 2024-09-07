namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delHomeFlag : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "HomeFlag");
            DropColumn("dbo.ProductCategories", "HomeFlag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductCategories", "HomeFlag", c => c.Boolean());
            AddColumn("dbo.Products", "HomeFlag", c => c.Boolean());
        }
    }
}
