namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addattributeToCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "ProductName", c => c.String());
            AddColumn("dbo.Carts", "totalProductPrice", c => c.String());
            AddColumn("dbo.Carts", "ProductQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "ProductQuantity");
            DropColumn("dbo.Carts", "totalProductPrice");
            DropColumn("dbo.Carts", "ProductName");
        }
    }
}
