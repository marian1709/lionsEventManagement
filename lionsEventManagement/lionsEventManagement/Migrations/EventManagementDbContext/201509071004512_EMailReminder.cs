namespace lionsEventManagement.Migrations.EventManagementDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EMailReminder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "FirstReminderMail", c => c.DateTime(nullable: false));
            AddColumn("dbo.Event", "SecondReminderMail", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "SecondReminderMail");
            DropColumn("dbo.Event", "FirstReminderMail");
        }
    }
}
