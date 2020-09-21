namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeCartIDTypeToOrderProductExtraItem : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderProductExtraItems", "CartID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderProductExtraItems", "CartID", c => c.Int(nullable: false));
        }
    }
}
