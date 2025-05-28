namespace TimeManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ToDoes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ToDoes", new[] { "UserId" });
            AlterColumn("dbo.ToDoes", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ToDoes", "UserId");
            AddForeignKey("dbo.ToDoes", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ToDoes", new[] { "UserId" });
            AlterColumn("dbo.ToDoes", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ToDoes", "UserId");
            AddForeignKey("dbo.ToDoes", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
