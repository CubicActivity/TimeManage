namespace TimeManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Goals",
                c => new
                    {
                        GoalId = c.Int(nullable: false, identity: true),
                        GoalName = c.String(),
                        GoalDescription = c.String(),
                    })
                .PrimaryKey(t => t.GoalId);
            
            AddColumn("dbo.Tasks", "Goal_GoalId", c => c.Int());
            CreateIndex("dbo.Tasks", "Goal_GoalId");
            AddForeignKey("dbo.Tasks", "Goal_GoalId", "dbo.Goals", "GoalId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Goal_GoalId", "dbo.Goals");
            DropIndex("dbo.Tasks", new[] { "Goal_GoalId" });
            DropColumn("dbo.Tasks", "Goal_GoalId");
            DropTable("dbo.Goals");
        }
    }
}
