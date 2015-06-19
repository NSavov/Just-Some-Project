using StudentRanking.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class Grader
    {
        private const String MATRICULARITY_EXAM = "матура";
        private const String DIPLOMA = "диплома";

        private RankingContext context;
        private Dictionary<string, double> grades;
        private Dictionary<string, string> useExam;

        public Grader(RankingContext context)
        {
            this.context = context;
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

        //private List<Exam> getStudentGrades(String studentEGN)
        //{
        //    List<Exam> grades = new List<Exam>();

        //    var query = from grade in context.Exams
        //                where grade.StudentEGN == studentEGN
        //                select grade;

        //    foreach (Exam grade in query)
        //    {
        //        grades.Add(grade);
        //    }

        //    return grades;
        //}

        private Dictionary<String, double> getStudentGrades(String studentEGN)
        {
            Dictionary<String, double> grades = new Dictionary<String, double>();

            var query = from grade in context.Exams
                        where grade.StudentEGN == studentEGN
                        select grade;

            foreach (Exam grade in query)
            {
                grades.Add(grade.ExamName, grade.Grade);
            }

            return grades;
        }

        private List<String> getMatricularityExams(List<List<String>> formulas)
        {
            List<String> matricularityExams = new List<string>();
            double value;

            for (int formulaInd = 0; formulaInd < formulas.Count; formulaInd++)
            {
                for (int componentInd = 1; componentInd < formulas[formulaInd].Count; componentInd += 2)
                {
                    if (formulas[formulaInd][componentInd].ToLower().Contains(MATRICULARITY_EXAM) && 
                        grades.TryGetValue(formulas[formulaInd][componentInd], out value))
                    {
                        String matricularityExam = formulas[formulaInd][componentInd].ToLower().Replace(MATRICULARITY_EXAM, String.Empty);
                        matricularityExam.Trim();
                        matricularityExams.Add(matricularityExam);
                    }

                }
            }

            return matricularityExams;
        }

        private Boolean hasMatricularityGrade(String exam, List<String> matricularityExams)
        {
            String filteredExamName = exam.ToLower().Replace(MATRICULARITY_EXAM, String.Empty);
            filteredExamName.Trim();

            if (matricularityExams.Contains(exam))
                return true;

            return false;
        }

        private double calculateTotalGrade(String StudentEGN, String programmeName)
        {
            //Boolean useMatExam = false; //should the matricularity exam be used instead of the diploma grade
            Double value;
            //TODO: return formulas applicable for this student

            List<List<String>> formulas = getFormulasComponents(programmeName);
            double totalGrade = 0;
            double maxGrade = 0;

            List<String> matricularityExams = getMatricularityExams(formulas);

            for (int formulaInd = 0; formulaInd < formulas.Count; formulaInd++)
            {

                for (int componentInd = 1; componentInd < formulas[formulaInd].Count; componentInd += 2)
                {
                    if (formulas[formulaInd][componentInd].ToLower().Contains(DIPLOMA) &&
                        hasMatricularityGrade(formulas[formulaInd][componentInd], matricularityExams))
                    {
                        //we shouldn't allow diploma grading if matricularity grade is available
                        break;
                    }

                    if (!grades.TryGetValue(formulas[formulaInd][componentInd], out value))
                    {
                        break;
                    }

                    int weight = Int32.Parse(formulas[formulaInd][componentInd - 1]);
                    double grade = grades[formulas[formulaInd][componentInd]];

                    totalGrade += weight * grade;
                }

                if (totalGrade > maxGrade)
                    maxGrade = totalGrade;

                totalGrade = 0;

            }

            return maxGrade;
        }


        public void grade(String studentEGN, List<Preference> preferences)
        {
            grades = getStudentGrades(studentEGN);

            foreach (Preference preference in preferences)
            {
                preference.TotalGrade = calculateTotalGrade(studentEGN, preference.ProgrammeName);

                //add total grade in preference table
                context.Preferences.Attach(preference);
                context.Entry(preference).Property(x => x.TotalGrade).IsModified = true;
                context.SaveChanges();
            }
        }
    }
}