namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricetypechangeToCart : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Carts", "totalProductPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Carts", "totalProductPrice", c => c.String());
        }
    }
}
