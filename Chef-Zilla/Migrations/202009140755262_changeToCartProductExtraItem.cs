namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeToCartProductExtraItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartProductExtraItems", "ExtraItemID", c => c.Int(nullable: false));
            DropColumn("dbo.CartProductExtraItems", "ProductID");
            DropColumn("dbo.CartProductExtraItems", "TotalPriceExtra");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartProductExtraItems", "TotalPriceExtra", c => c.Int(nullable: false));
            AddColumn("dbo.CartProductExtraItems", "ProductID", c => c.Int(nullable: false));
            DropColumn("dbo.CartProductExtraItems", "ExtraItemID");
        }
    }
}
