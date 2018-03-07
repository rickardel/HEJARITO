namespace HEJARITO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validationattributeonactivityType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActivityTypes", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActivityTypes", "Name", c => c.String());
        }
    }
}
