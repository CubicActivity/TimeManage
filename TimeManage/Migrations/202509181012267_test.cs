namespace TimeManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "PomodoroTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "PomodoroTime", c => c.Int(nullable: false));
        }
    }
}
