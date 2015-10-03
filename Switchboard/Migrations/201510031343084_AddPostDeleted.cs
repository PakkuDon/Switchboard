namespace Switchboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Deleted");
        }
    }
}
