namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductModelCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtraIngredients",
                c => new
                    {
                        ExtraIngredientID = c.Int(nullable: false, identity: true),
                        ExtraIngredientName = c.String(),
                        ExtraIngredientPrice = c.String(),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExtraIngredientID);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        IngredientID = c.Int(nullable: false, identity: true),
                        IngredientName = c.String(),
                        IngredientQuantity = c.String(),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IngredientID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductType = c.String(),
                        ProductName = c.String(),
                        ProductImage = c.String(),
                        PrepareTime = c.String(),
                        ProductPrice = c.String(),
                    })
                .PrimaryKey(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
            DropTable("dbo.Ingredients");
            DropTable("dbo.ExtraIngredients");
        }
    }
}
