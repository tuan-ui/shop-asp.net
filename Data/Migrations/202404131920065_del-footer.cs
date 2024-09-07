namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delfooter : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Footers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Footers",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
