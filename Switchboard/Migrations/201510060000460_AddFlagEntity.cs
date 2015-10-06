namespace Switchboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlagEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Flags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                        ReportedOn = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        PostID = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.PostID)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Flags", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Flags", "PostID", "dbo.Posts");
            DropIndex("dbo.Flags", new[] { "User_Id" });
            DropIndex("dbo.Flags", new[] { "PostID" });
            DropTable("dbo.Flags");
        }
    }
}
