using StudentRanking.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<Preference> getStudentPreferences(String EGN)
        {
            List<Preference> preferences = new List<Preference>();

            var query = from pref in context.Preferences
                        where pref.EGN == EGN
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
                students.Add(new RankListEntry(student.EGN, student.TotalGrade));
            }

            return students;
        }

        private String getFaculty(String programmeName)
        {
            var query = from ProgrammeToFaculty in context.Faculties
                        where ProgrammeToFaculty.ProgrammeName == programmeName
                        select ProgrammeToFaculty.FacultyName;

            return query.First();
        }

        private List<List<String>> getFormulasComponents(String programmeName)
        {
            List<List<String>> allComponents = new List<List<string>>();
            List<String> components = new List<string>();

            var query = from formula in context.Formulas
                        where formula.ProgrammeName == programmeName
                        select formula;

            foreach (var formula in query)
            {
                if (formula.C1 > 0)
                {
                    components.Add(formula.C1.ToString());
                    components.Add(formula.X);
                }

                if (formula.C2 > 0)
                {
                    components.Add(formula.C2.ToString());
                    components.Add(formula.Y);
                }

                if (formula.C3 > 0)
                {
                    components.Add(formula.C3.ToString());
                    components.Add(formula.Z);
                }

                if (formula.C4 > 0)
                {
                    components.Add(formula.C4.ToString());
                    components.Add(formula.W);
                }

                allComponents.Add(components);
            }

            return allComponents;
        }

        private List<Exam> getStudentGrades(String studentEGN)
        {
            List<Exam> grades = new List<Exam>();

            var query = from grade in context.Exams
                        where grade.StudentEGN == studentEGN
                        select grade;

            foreach (Exam grade in query)
            {
                grades.Add(grade);
            }

            return grades;
        }

        private List<List<String>> filterStudentFormulas(String StudentEGN, String programmeName)
        {
            Boolean useMatExam = false; //should the matricularity exam be used instead of the diploma grade

            //TODO: return formulas applicable for this student

            List<List<String>> formulas = getFormulasComponents(programmeName);

            for (int formulaInd = 0; formulaInd < formulas.Count; formulaInd++)
            {

                for (int componentInd = 1; componentInd < formulas[formulaInd].Count; componentInd += 2)
                {
                   // if (!hasExamGrade())
                    {
                        formulas.RemoveAt(formulaInd);
                        break;
                    }
                }

            }

            return formulas;
        }

        private void calculateTotalGrade(String studentEGN, List<Preference> preferences)
        {
            List<Exam> grades = getStudentGrades(studentEGN);

            foreach(Preference preference in preferences)
            {
                //add total grade in preference table
            }
        }

        private Dictionary<String, List<Preference>> splitPreferencesByFaculty(List<Preference> preferences)
        {
            Dictionary<String, List<Preference>> splittedPreferences = new Dictionary<String, List<Preference>>();
            List<Preference> value;

            foreach (Preference preference in preferences)
            {
                String faculty = getFaculty(preference.ProgrammeName);


                if (!splittedPreferences.TryGetValue(faculty, out value))
                {
                    splittedPreferences.Add(faculty, new List<Preference>());
                }

                splittedPreferences[faculty].Add(preference);
            }

            return splittedPreferences;
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
                List<Preference> allPreferences = getStudentPreferences(students[i].EGN);
                Dictionary<String, List<Preference>> preferences = splitPreferencesByFaculty(allPreferences);

                //handle grading

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

            var pref = this.getStudentPreferences("1234121");

            Debug.WriteLine("test");

            foreach (Preference preference in pref)
                Debug.WriteLine(preference.ProgrammeName);
        }


    }
}