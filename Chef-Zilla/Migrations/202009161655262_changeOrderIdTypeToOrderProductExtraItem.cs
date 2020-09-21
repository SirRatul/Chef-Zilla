namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeOrderIdTypeToOrderProductExtraItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderProductExtraItems", "OrderId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderProductExtraItems", "OrderId");
        }
    }
}
