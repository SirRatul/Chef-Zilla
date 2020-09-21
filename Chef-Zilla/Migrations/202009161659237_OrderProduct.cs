namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        OrderProductId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderProductId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderProducts");
        }
    }
}
