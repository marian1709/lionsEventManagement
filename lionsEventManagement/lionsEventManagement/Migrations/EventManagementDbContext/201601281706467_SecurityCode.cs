namespace lionsEventManagement.Migrations.EventManagementDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecurityCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Participant", "SecurityCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Participant", "SecurityCode");
        }
    }
}
