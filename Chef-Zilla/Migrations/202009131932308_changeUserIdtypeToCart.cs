namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeUserIdtypeToCart : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Carts", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Carts", "UserId", c => c.String());
        }
    }
}
