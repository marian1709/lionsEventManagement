namespace lionsEventManagement.Migrations.EventManagementDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ScheduledEmail", "ScheduledDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ScheduledEmail", "ScheduledDate", c => c.DateTime(nullable: false));
        }
    }
}
