namespace lionsEventManagement.Migrations.EventManagementDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimetoEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "Time");
        }
    }
}
