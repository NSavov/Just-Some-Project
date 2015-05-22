using StudentRanking.Filters;
using StudentRanking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace StudentRanking.DataAccess
{
    public class RankingContext : DbContext

    {
        public RankingContext()
            : base("DefaultConnection")
        { }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<ProgrammeRules> ProgrammesRules { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public Dictionary<String, DbSet<FacultyRankList>> FacultyRankLists { get; set; }
        protected void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new IndexInitializer<DbContext>());
        }
    }
}