namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "dateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "status");
            DropColumn("dbo.Orders", "dateTime");
        }
    }
}
