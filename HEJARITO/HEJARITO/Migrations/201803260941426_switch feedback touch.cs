namespace HEJARITO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class switchfeedbacktouch : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Feedbacks", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Feedbacks", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Feedbacks", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Feedbacks", "ApplicationUserId");
            AddForeignKey("dbo.Feedbacks", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Feedbacks", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Feedbacks", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Feedbacks", "ApplicationUserId");
            AddForeignKey("dbo.Feedbacks", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
