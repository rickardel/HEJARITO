namespace HEJARITO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredfilename : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "FileName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.StudentDocuments", "FileName", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StudentDocuments", "FileName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Documents", "FileName", c => c.String(maxLength: 255));
        }
    }
}
