namespace StudentRanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Preferences", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Preferences", "IsApproved");
        }
    }
}
