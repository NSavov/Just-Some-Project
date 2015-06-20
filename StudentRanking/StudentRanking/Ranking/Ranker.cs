using StudentRanking.DataAccess;
using StudentRanking.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace StudentRanking.Ranking
{
    public class Ranker
    {

        private RankingContext context;
        private QueryManager queryManager;
        private struct RankListData
        {
            public int studentCount;
            public double minimalGrade;
        }

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
            queryManager = new QueryManager(context);
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
                students.Add(new RankListEntry(student.EGN, student.TotalGrade));
            }

            return students;
        }





        private Dictionary<String, List<Preference>> splitPreferencesByFaculty(List<Preference> preferences)
        {
            Dictionary<String, List<Preference>> splittedPreferences = new Dictionary<String, List<Preference>>();
            List<Preference> value;

            foreach (Preference preference in preferences)
            {
                String faculty = queryManager.getFaculty(preference.ProgrammeName);


                if (!splittedPreferences.TryGetValue(faculty, out value))
                {
                    splittedPreferences.Add(faculty, new List<Preference>());
                }

                splittedPreferences[faculty].Add(preference);
            }

            return splittedPreferences;
        }

        private RankListData getRankListData(String programmeName)
        {

            RankListData data = new RankListData();
            var query = from rankEntry in context.FacultyRankLists
                        where rankEntry.ProgrammeName == programmeName
                        orderby rankEntry.TotalGrade ascending
                        select rankEntry;

            data.studentCount = query.Count();
            data.minimalGrade = query.First().TotalGrade;

            return data;
        }



        private void match(String facultyName, List<Preference> preferences, Boolean gender)
        {
            //TODO: decide what to do with expelled students
            foreach (Preference preference in preferences)
            {
                int quota = queryManager.getQuota(preference.ProgrammeName, gender);
                RankListData data = getRankListData(preference.ProgrammeName);

                if (preference.TotalGrade > data.minimalGrade &&
                    data.studentCount >= quota)
                {

                    var entries = context.FacultyRankLists.Where(entry => entry.TotalGrade == data.minimalGrade);

                    foreach (FacultyRankList entry in entries)
                    {
                        context.FacultyRankLists.Remove(entry);
                    }
                    context.SaveChanges();
                }

                if (preference.TotalGrade >= data.minimalGrade ||
                    (preference.TotalGrade < data.minimalGrade && data.studentCount < quota))
                {
                    FacultyRankList entry = new FacultyRankList()
                        {
                            EGN = preference.EGN,
                            ProgrammeName = preference.ProgrammeName,
                            TotalGrade = preference.TotalGrade
                        };

                    context.FacultyRankLists.Add(entry);
                    context.SaveChanges();

                    break;
                }

            }
        }

        public void start()
        {
            List<Student> students = new List<Student>();

            //getting all unenrolled students
            var getStudentsQuery = from student in context.Students
                                   where student.IsEnrolled == false
                                   select student;

            foreach (var student in getStudentsQuery)
            {
                students.Add(student);
            }

            for (int i = 0; i < students.Count; i++)
            {
                //handle preferences
                List<Preference> allPreferences = queryManager.getStudentPreferences(students[i].EGN);
                Dictionary<String, List<Preference>> preferences = splitPreferencesByFaculty(allPreferences);

                //handle grading
                Grader grader = new Grader(context);
                grader.grade(students[i].EGN, allPreferences);

                //handle ranking
                foreach (String key in preferences.Keys)
                {
                    match(key, preferences[key], (Boolean)students[i].Gender);
                }
            }
        }

        public void test()
        {
            //Preference pref1 = new Preference(){EGN = "1234121", IsApproved = false, PrefNumber = 1, ProgrammeName = "CS"};
            //Preference pref2 = new Preference() { EGN = "1234121", IsApproved = false, PrefNumber = 2, ProgrammeName = "ST" };
            //Student s = new Student() {EGN="123445", Email="asdf", FirstName="A", Gender=true, IsEnrolled = false, LastName = "L" };
            //Student s2 = new Student() { EGN = "123345", Email = "asdaf", FirstName = "A1", Gender = true, IsEnrolled = false, LastName = "L1" };
            ////context.Students.Add(s);
            ////context.Students.Add(s2);
            ////context.SaveChanges();
            //context.Preferences.Add(pref1);
            //context.Preferences.Add(pref2);
            //context.SaveChanges();

            var pref = queryManager.getStudentPreferences("1234121");

            Debug.WriteLine("test");

            foreach (Preference preference in pref)
                Debug.WriteLine(preference.ProgrammeName);
        }


    }
}