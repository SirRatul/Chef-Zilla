namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTotalItemToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "totalItem", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "totalItem");
        }
    }
}
