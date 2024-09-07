namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDatePromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotions", "DateStart", c => c.DateTime());
            AddColumn("dbo.Promotions", "DateEnd", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promotions", "DateEnd");
            DropColumn("dbo.Promotions", "DateStart");
        }
    }
}
