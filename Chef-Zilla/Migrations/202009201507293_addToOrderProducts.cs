namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addToOrderProducts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderProducts", "ProductPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderProducts", "ProductPrice");
        }
    }
}
