namespace Switchboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlagValidationAndResponseField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flags", "Response", c => c.String());
            AlterColumn("dbo.Flags", "Reason", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Flags", "Reason", c => c.String());
            DropColumn("dbo.Flags", "Response");
        }
    }
}
