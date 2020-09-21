namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeToOrder1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "dateTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "dateTime", c => c.DateTime(nullable: false));
        }
    }
}
