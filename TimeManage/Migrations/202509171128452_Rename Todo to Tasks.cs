namespace TimeManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTodotoTasks : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ToDoes", newName: "Tasks");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Tasks", newName: "ToDoes");
        }
    }
}
