namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteImage : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProductCategories", "Image");
            DropColumn("dbo.Supplier", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Supplier", "Image", c => c.String(maxLength: 256));
            AddColumn("dbo.ProductCategories", "Image", c => c.String(maxLength: 256));
        }
    }
}
