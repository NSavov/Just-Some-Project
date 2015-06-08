namespace StudentRanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "IsEnrolled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Preferences", "IsApproved", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Students", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Gender", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Preferences", "ProgrammeName", c => c.String(nullable: false));
            AlterColumn("dbo.Formulae", "ProgrammeName", c => c.String(nullable: false));
            AlterColumn("dbo.Faculties", "FacultyName", c => c.String(nullable: false));
            DropColumn("dbo.Students", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "State", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Faculties", "FacultyName", c => c.String());
            AlterColumn("dbo.Formulae", "ProgrammeName", c => c.String());
            AlterColumn("dbo.Preferences", "ProgrammeName", c => c.String());
            AlterColumn("dbo.Students", "Gender", c => c.Boolean());
            AlterColumn("dbo.Students", "Email", c => c.String());
            AlterColumn("dbo.Students", "LastName", c => c.String());
            AlterColumn("dbo.Students", "FirstName", c => c.String());
            DropColumn("dbo.Preferences", "IsApproved");
            DropColumn("dbo.Students", "IsEnrolled");
        }
    }
}
