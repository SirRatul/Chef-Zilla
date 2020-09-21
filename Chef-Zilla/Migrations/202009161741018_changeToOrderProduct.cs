namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeToOrderProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderProducts", "ProductQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderProducts", "ProductQuantity");
        }
    }
}
