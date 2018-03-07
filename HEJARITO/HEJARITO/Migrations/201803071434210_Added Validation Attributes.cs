namespace HEJARITO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedValidationAttributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Modules", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Activities", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Activities", "Name", c => c.String());
            AlterColumn("dbo.Modules", "Name", c => c.String());
            AlterColumn("dbo.Courses", "Name", c => c.String());
        }
    }
}
