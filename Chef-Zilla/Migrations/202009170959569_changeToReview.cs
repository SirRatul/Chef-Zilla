namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeToReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "ReviewText", c => c.String());
            DropColumn("dbo.Reviews", "ReviewTexr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "ReviewTexr", c => c.String());
            DropColumn("dbo.Reviews", "ReviewText");
        }
    }
}
