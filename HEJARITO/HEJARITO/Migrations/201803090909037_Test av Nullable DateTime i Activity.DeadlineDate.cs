namespace HEJARITO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestavNullableDateTimeiActivityDeadlineDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "DeadlineDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Activities", "DeadlineDate", c => c.DateTime(nullable: false));
        }
    }
}
