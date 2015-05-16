namespace StudentRanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        EGN = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Gender = c.Boolean(nullable: false),
                        State = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EGN);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students");
        }
    }
}
