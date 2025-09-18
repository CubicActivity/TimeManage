namespace TimeManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Goals", "GoalName", c => c.String(nullable: false));
            AlterColumn("dbo.Goals", "GoalDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Goals", "GoalDescription", c => c.String());
            AlterColumn("dbo.Goals", "GoalName", c => c.String());
        }
    }
}
