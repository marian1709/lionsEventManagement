namespace lionsEventManagement.Migrations.EventManagementDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mandator",
                c => new
                    {
                        MandantId = c.Int(nullable: false, identity: true),
                        MandantName = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MandantId);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        MemberId = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DateEdited = c.DateTime(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Mandator_MandantId = c.Int(),
                    })
                .PrimaryKey(t => t.MemberId)
                .ForeignKey("dbo.Mandator", t => t.Mandator_MandantId)
                .Index(t => t.Mandator_MandantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Member", "Mandator_MandantId", "dbo.Mandator");
            DropIndex("dbo.Member", new[] { "Mandator_MandantId" });
            DropTable("dbo.Member");
            DropTable("dbo.Mandator");
        }
    }
}
