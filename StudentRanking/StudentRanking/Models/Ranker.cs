using StudentRanking.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class Ranker
    {
        RankingContext context;

        struct RankListEntry
        {
            public RankListEntry(String studentEGN, double totalGrade)
            {
                this.studentEGN = studentEGN;
                this.totalGrade = totalGrade;
            }

            public String studentEGN;
            public double totalGrade;
        }

        public Ranker(RankingContext context)
        {
            this.context = context;
        }

        //Returns a list of preferences of a student by SSN
        private List<Preference> getStudentPreferences(Student student)
        {
            List<Preference> preferences = new List<Preference>();

            var query = from pref in context.Preferences
                        where pref.EGN == student.EGN
                        select pref;

            foreach (var pref in query)
            {
                preferences.Add(pref);
            }

            return preferences;
        }

        //Returns a list of student SSNs currently in a Programme by Programme
        private List<RankListEntry> getProgrammeStudents(String ProgrammeName)
        {
            List<RankListEntry> students = new List<RankListEntry>();

            var query = from student in context.FacultyRankLists
                        where student.ProgrammeName == ProgrammeName
                        select student;

            foreach (var student in query)
            {
                students.Add(new RankListEntry(student.EGN, student.TotalGrade) );
            }

            return students;
        }

        public void test()
        {
            
        }

    }
}