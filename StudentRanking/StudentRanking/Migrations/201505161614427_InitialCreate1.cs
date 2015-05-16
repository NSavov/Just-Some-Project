namespace StudentRanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        ExamName = c.String(nullable: false, maxLength: 128),
                        StudentEGN = c.String(nullable: false, maxLength: 128),
                        Grade = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.ExamName, t.StudentEGN });
            
            CreateTable(
                "dbo.ProgrammeRules",
                c => new
                    {
                        ProgrammeName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ProgrammeName);
            
            CreateTable(
                "dbo.Preferences",
                c => new
                    {
                        EGN = c.String(nullable: false, maxLength: 128),
                        PrefNumber = c.Int(nullable: false),
                        ProgrammeName = c.String(),
                    })
                .PrimaryKey(t => new { t.EGN, t.PrefNumber });
            
            CreateTable(
                "dbo.Formulae",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgrammeName = c.String(),
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
                "dbo.Faculties",
                c => new
                    {
                        ProgrammeName = c.String(nullable: false, maxLength: 128),
                        FacultyName = c.String(),
                    })
                .PrimaryKey(t => t.ProgrammeName);
            
            AlterColumn("dbo.Students", "Gender", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "Gender", c => c.Boolean(nullable: false));
            DropTable("dbo.Faculties");
            DropTable("dbo.Formulae");
            DropTable("dbo.Preferences");
            DropTable("dbo.ProgrammeRules");
            DropTable("dbo.Exams");
        }
    }
}
