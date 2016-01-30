namespace lionsEventManagement.Migrations.EventManagementDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AcceptanceEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "AcceptanceEvent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "AcceptanceEvent");
        }
    }
}
