namespace StudentRanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            DropTable("dbo.Exams");
            DropTable("dbo.Students");
            DropTable("dbo.ProgrammeRules");
            DropTable("dbo.Preferences");
            DropTable("dbo.Formulae");
            DropTable("dbo.Faculties");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        ProgrammeName = c.String(nullable: false, maxLength: 128),
                        FacultyName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ProgrammeName);
            
            CreateTable(
                "dbo.Formulae",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgrammeName = c.String(nullable: false),
                        C1 = c.Double(nullable: false),
                        X = c.String(),
                        C2 = c.Double(nullable: false),
                        Y = c.String(),
                        C3 = c.Double(nullable: false),
                        Z = c.String(),
                        C4 = c.Double(nullable: false),
                        W = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Preferences",
                c => new
                    {
                        EGN = c.String(nullable: false, maxLength: 128),
                        PrefNumber = c.Int(nullable: false),
                        ProgrammeName = c.String(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.EGN, t.PrefNumber });
            
            CreateTable(
                "dbo.ProgrammeRules",
                c => new
                    {
                        ProgrammeName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ProgrammeName);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        EGN = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Gender = c.Boolean(nullable: false),
                        IsEnrolled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EGN);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        ExamName = c.String(nullable: false, maxLength: 128),
                        StudentEGN = c.String(nullable: false, maxLength: 128),
                        Grade = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.ExamName, t.StudentEGN });
            
            DropTable("dbo.UserProfile");
        }
    }
}
