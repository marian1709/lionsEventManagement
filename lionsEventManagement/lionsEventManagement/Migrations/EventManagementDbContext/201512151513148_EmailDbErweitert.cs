namespace lionsEventManagement.Migrations.EventManagementDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailDbErweitert : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScheduledEmail", "AnswerCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScheduledEmail", "AnswerCode");
        }
    }
}
