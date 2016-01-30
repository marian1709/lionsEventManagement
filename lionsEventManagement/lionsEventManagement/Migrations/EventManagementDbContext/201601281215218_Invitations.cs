namespace lionsEventManagement.Migrations.EventManagementDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invitations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Member", "Event_EventId", "dbo.Event");
            DropForeignKey("dbo.Member", "Mandator_MandantId", "dbo.Mandator");
            DropIndex("dbo.Member", new[] { "Event_EventId" });
            DropIndex("dbo.Member", new[] { "Mandator_MandantId" });
            CreateTable(
                "dbo.Participant",
                c => new
                    {
                        ParticipantId = c.Int(nullable: false, identity: true),
                        Participating = c.Int(nullable: false),
                        Member_MemberId = c.Int(),
                        Event_EventId = c.Int(),
                    })
                .PrimaryKey(t => t.ParticipantId)
                .ForeignKey("dbo.Member", t => t.Member_MemberId)
                .ForeignKey("dbo.Event", t => t.Event_EventId)
                .Index(t => t.Member_MemberId)
                .Index(t => t.Event_EventId);
            
            AddColumn("dbo.ScheduledEmail", "ParticipantId", c => c.Int(nullable: false));
            DropColumn("dbo.Member", "Event_EventId");
            DropColumn("dbo.Member", "Mandator_MandantId");
            DropTable("dbo.Mandator");
        }
        
        public override void Down()
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
            
            AddColumn("dbo.Member", "Mandator_MandantId", c => c.Int());
            AddColumn("dbo.Member", "Event_EventId", c => c.Int());
            DropForeignKey("dbo.Participant", "Event_EventId", "dbo.Event");
            DropForeignKey("dbo.Participant", "Member_MemberId", "dbo.Member");
            DropIndex("dbo.Participant", new[] { "Event_EventId" });
            DropIndex("dbo.Participant", new[] { "Member_MemberId" });
            DropColumn("dbo.ScheduledEmail", "ParticipantId");
            DropTable("dbo.Participant");
            CreateIndex("dbo.Member", "Mandator_MandantId");
            CreateIndex("dbo.Member", "Event_EventId");
            AddForeignKey("dbo.Member", "Mandator_MandantId", "dbo.Mandator", "MandantId");
            AddForeignKey("dbo.Member", "Event_EventId", "dbo.Event", "EventId");
        }
    }
}
