namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeToReveiw : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reviews", "OrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "OrderId", c => c.Int(nullable: false));
        }
    }
}
