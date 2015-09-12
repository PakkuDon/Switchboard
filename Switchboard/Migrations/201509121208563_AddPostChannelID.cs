namespace Switchboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostChannelID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "Channel_ID", "dbo.Channels");
            DropIndex("dbo.Posts", new[] { "Channel_ID" });
            RenameColumn(table: "dbo.Posts", name: "Channel_ID", newName: "ChannelID");
            AlterColumn("dbo.Posts", "ChannelID", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "ChannelID");
            AddForeignKey("dbo.Posts", "ChannelID", "dbo.Channels", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ChannelID", "dbo.Channels");
            DropIndex("dbo.Posts", new[] { "ChannelID" });
            AlterColumn("dbo.Posts", "ChannelID", c => c.Int());
            RenameColumn(table: "dbo.Posts", name: "ChannelID", newName: "Channel_ID");
            CreateIndex("dbo.Posts", "Channel_ID");
            AddForeignKey("dbo.Posts", "Channel_ID", "dbo.Channels", "ID");
        }
    }
}
