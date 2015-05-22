namespace StudentRanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "IsEnrolled", c => c.Boolean(nullable: false));
            DropColumn("dbo.Students", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "State", c => c.Boolean(nullable: false));
            DropColumn("dbo.Students", "IsEnrolled");
        }
    }
}
