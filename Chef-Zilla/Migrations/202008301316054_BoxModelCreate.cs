namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoxModelCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boxes",
                c => new
                    {
                        BoxID = c.Int(nullable: false, identity: true),
                        BoxName = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.BoxID);
            
            CreateTable(
                "dbo.BoxExtraItems",
                c => new
                    {
                        BoxExtraItemID = c.Int(nullable: false, identity: true),
                        ExtraIngredientID = c.Int(nullable: false),
                        ExtraIngredientQuantity = c.String(),
                        BoxID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BoxExtraItemID);
            
            CreateTable(
                "dbo.BoxProducts",
                c => new
                    {
                        BoxProductID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        ProductQuantity = c.Int(nullable: false),
                        TotalPrice = c.Int(nullable: false),
                        BoxID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BoxProductID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BoxProducts");
            DropTable("dbo.BoxExtraItems");
            DropTable("dbo.Boxes");
        }
    }
}
