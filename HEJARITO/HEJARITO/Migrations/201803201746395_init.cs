namespace HEJARITO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "ActivityId", "dbo.Activities");
            AddColumn("dbo.Documents", "Activity_Id", c => c.Int());
            AddColumn("dbo.Documents", "Activity_Id1", c => c.Int());
            CreateIndex("dbo.Documents", "Activity_Id");
            CreateIndex("dbo.Documents", "Activity_Id1");
            AddForeignKey("dbo.Documents", "Activity_Id1", "dbo.Activities", "Id");
            AddForeignKey("dbo.Documents", "Activity_Id", "dbo.Activities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "Activity_Id", "dbo.Activities");
            DropForeignKey("dbo.Documents", "Activity_Id1", "dbo.Activities");
            DropIndex("dbo.Documents", new[] { "Activity_Id1" });
            DropIndex("dbo.Documents", new[] { "Activity_Id" });
            DropColumn("dbo.Documents", "Activity_Id1");
            DropColumn("dbo.Documents", "Activity_Id");
            AddForeignKey("dbo.Documents", "ActivityId", "dbo.Activities", "Id");
        }
    }
}
