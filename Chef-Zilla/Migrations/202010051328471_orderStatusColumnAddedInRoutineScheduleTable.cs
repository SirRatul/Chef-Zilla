namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderStatusColumnAddedInRoutineScheduleTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoutineSchedules", "OrderStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoutineSchedules", "OrderStatus");
        }
    }
}
