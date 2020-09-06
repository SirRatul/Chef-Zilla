namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoutineModelCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Routines",
                c => new
                    {
                        RoutineID = c.Int(nullable: false, identity: true),
                        RoutineName = c.String(),
                        BoxID = c.Int(nullable: false),
                        UserId = c.String(),
                        DeliveryAddress = c.String(),
                        RoutineStatus = c.String(),
                    })
                .PrimaryKey(t => t.RoutineID);
            
            CreateTable(
                "dbo.RoutineSchedules",
                c => new
                    {
                        RoutineScheduleID = c.Int(nullable: false, identity: true),
                        RoutineType = c.String(),
                        DeliveryDay = c.String(),
                        DeliveryDate = c.String(),
                        RoutineID = c.Int(nullable: false),
                        DeliveredDate = c.String(),
                    })
                .PrimaryKey(t => t.RoutineScheduleID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RoutineSchedules");
            DropTable("dbo.Routines");
        }
    }
}
