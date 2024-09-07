namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delSlide : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Slides");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                        Image = c.String(maxLength: 256),
                        Url = c.String(maxLength: 256),
                        DisplayOrder = c.Int(),
                        Status = c.Boolean(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
