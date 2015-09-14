namespace Switchboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetPostUserAsNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "User_Id" });
            AlterColumn("dbo.Posts", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "User_Id");
            AddForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "User_Id" });
            AlterColumn("dbo.Posts", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Posts", "User_Id");
            AddForeignKey("dbo.Posts", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
