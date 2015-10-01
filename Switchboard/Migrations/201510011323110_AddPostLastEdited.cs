namespace Switchboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostLastEdited : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "LastEdited", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "LastEdited");
        }
    }
}
