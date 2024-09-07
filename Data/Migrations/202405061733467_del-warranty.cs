namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delwarranty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Warranty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Warranty", c => c.Int());
        }
    }
}
