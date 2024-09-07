namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addExpiredDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "ExpiredDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "ExpiredDate");
        }
    }
}
