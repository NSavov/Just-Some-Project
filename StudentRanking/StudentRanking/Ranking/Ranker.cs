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
        private const String CONST_REJECTED = "rejected";

        private RankingContext context;
        private QueryManager queryManager;

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

        private void match(String facultyName, List<Preference> preferences, Student student)
        {
            //add student as rejected in the db at first
            FacultyRankList rejected = new FacultyRankList()
            {
                EGN = student.EGN,
                ProgrammeName = CONST_REJECTED + ' ' + facultyName,
                TotalGrade = 0
            };

            context.FacultyRankLists.Add(rejected);
            context.SaveChanges();

            foreach (Preference preference in preferences)
            {
                int quota = queryManager.getQuota(preference.ProgrammeName, (bool)student.Gender);
                List<FacultyRankList> rankList = queryManager.getRankListData(preference.ProgrammeName, (bool)student.Gender);
                double minimalGrade = rankList.Min(list => list.TotalGrade);
                double studentCount = rankList.Count;

                //TODO: Think of a smarter way to delete students
                if (preference.TotalGrade > minimalGrade &&
                    studentCount >= quota)
                {

                    var entries = rankList.Where(entry => entry.TotalGrade == minimalGrade);

                    foreach (FacultyRankList entry in entries)
                    {
                        context.FacultyRankLists.Attach(entry);
                        context.FacultyRankLists.Remove(entry);

                    }
                    context.SaveChanges();
                }

                if (preference.TotalGrade >= minimalGrade ||
                    (preference.TotalGrade < minimalGrade && studentCount < quota))
                {
                    FacultyRankList entry = new FacultyRankList()
                        {
                            EGN = preference.EGN,
                            ProgrammeName = preference.ProgrammeName,
                            TotalGrade = preference.TotalGrade
                        };

                    context.FacultyRankLists.Add(entry);
                    context.FacultyRankLists.Attach(rejected);
                    context.FacultyRankLists.Remove(rejected);
                    context.SaveChanges();

                    break;
                }

            }
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

        //returns a list of students that are not rejected and were pushed away from their preferred programme
        private List<Student> getUnmatchedStudents()
        {
            List<Student> unmatched = new List<Student>();

            var getStudentQuery = from student in context.Students
                                  select student.EGN;

            var getEntriesQuery = from entry in context.FacultyRankLists
                                  select entry.EGN;

            var unmatchedS = getStudentQuery.Except(getEntriesQuery);
            return unmatched;
        }

        private void serve(Student student)
        {
            //handle preferences
            List<Preference> allPreferences = queryManager.getStudentPreferences(student.EGN);
            Dictionary<String, List<Preference>> preferences = splitPreferencesByFaculty(allPreferences);

            //handle grading
            Grader grader = new Grader(context);
            grader.grade(student.EGN, allPreferences);

            //handle ranking
            foreach (String key in preferences.Keys)
            {
                match(key, preferences[key], student);
            }
        }

        public void start()
        {
            List<Student> students = new List<Student>();

            //getting all unenrolled students
            //var getStudentsQuery = from student in context.Students
            //                       where student.IsEnrolled == false
            //                       select student;

            //foreach (var student in getStudentsQuery)
            //{
            //    serve(student);

            //}


            var getFacultyNames = (from faculty in context.Faculties
                                  select faculty.FacultyName).Distinct();

            List<String> facultyNames = getFacultyNames.ToList();
            List<String> studentEGNs;

            foreach (String facultyName in facultyNames)
            {
                var getStudentsEGNQuery = (from student in context.Students
                                          from preference in context.Preferences
                                          from faculty in context.Faculties
                                          where student.IsEnrolled == false
                                          where faculty.FacultyName == facultyName && preference.ProgrammeName == faculty.ProgrammeName
                                          select student.EGN).Distinct();



                var getApprovedStudentsEGNQuery = (from entry in context.FacultyRankLists
                                                  from faculty in context.Faculties
                                                  where entry.ProgrammeName == faculty.ProgrammeName || (faculty.ProgrammeName.Equals( CONST_REJECTED + " " + faculty.FacultyName))
                                                  where faculty.FacultyName == facultyName
                                                  select entry.EGN).Distinct();

                //var unmatchedEGNQuery = getStudentsEGNQuery.Where(egn => !egn.Equals(getApprovedStudentsEGNQuery.Any()));
                //from egn in getStudentsEGNQuery//getStudentsEGNQuery.Except(getApprovedStudentsEGNQuery.ToArray());
                //where !getApprovedStudentsEGNQuery.Any().Equals(egn)
                //select egn;


                int count;

                do
                {
                    studentEGNs = getStudentsEGNQuery.ToList();
                    count = 0;
                    foreach (String EGN in studentEGNs)
                    {
                        Student student;
                        student = queryManager.getStudent(EGN);

                        //bool isApproved = getApprovedStudentsEGNQuery.Where(studentEGN => studentEGN == EGN).Count() > 0;

                        if (!getApprovedStudentsEGNQuery.Contains(EGN))
                        { 
                            serve(student);
                            count++;
                        }
                    }
                    
                }
                while (count > 0);
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
            var student = queryManager.getStudent("1231231231");

            //handle preferences
            List<Preference> allPreferences = queryManager.getStudentPreferences(student.EGN);
            Dictionary<String, List<Preference>> preferences = splitPreferencesByFaculty(allPreferences);

            //handle grading
            Grader grader = new Grader(context);
            grader.grade(student.EGN, allPreferences);

            foreach (String key in preferences.Keys)
            {
                match(key, preferences[key], student);
            }

            //test preference handling
            ////List<Preference> allPreferences = queryManager.getStudentPreferences(student.EGN);
            ////Dictionary<String, List<Preference>> preferences = splitPreferencesByFaculty(allPreferences);


            ////foreach (Preference preference in allPreferences)
            ////    Debug.Write(preference.ProgrammeName + ' ');

            ////Debug.WriteLine(0);

            ////foreach(String key in preferences.Keys)
            ////{
            ////    foreach (Preference preference in preferences[key])
            ////        Debug.Write(preference.ProgrammeName + ' ');

            ////    Debug.WriteLine(0);
            ////}
        }


    }
}