using StudentRanking.DataAccess;
using StudentRanking.Ranking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentRanking.Models
{
    public class StudentRankingInformationController : Controller
    {
        //
        // GET: /StudentRankingInformation/
        private RankingContext db = new RankingContext();
        private Dictionary<String, List<String>> programmes = new Dictionary<String, List<String>>();
        private List<StudentRankingInformation> model = new List<StudentRankingInformation>();

        private List<String> getProgrammes(List<FacultyRankList> l)
        {
            List<String> result = new List<String>();
            foreach (var item in l)
            {
                result.Add(item.ProgrammeName);
            }
            return result;
        }

        //I found that User works, ie. User.Identity.Name or User.IsInRole("Administrator")...
        public ActionResult Index()
        {


            String user = User.Identity.Name;
            ViewData["userName"] = user;

            bool isEnrolled = false;
            Student st = db.Students.Find(user);
            if ( st != null )
            {
                isEnrolled = st.IsEnrolled;
            }

            if (isEnrolled)
            {
                ViewData["isEnrolled"] = true;
                return View(model);
            }

            ViewData["isEnrolled"] = false;

            QueryManager queryManager = new QueryManager(db);

            List<FacultyRankList> studentRankList = queryManager.getStudentRankList(user);

            List<String> l = getProgrammes(studentRankList);
            l.Insert(0, "Please Select");
            SelectList pr = new SelectList(l);

            ViewData["programmes"] = pr;



            List<String> studentProgrammes = new List<String>();
            foreach (var item in studentRankList)
            {
                studentProgrammes.Add(item.ProgrammeName);
            }

            Dictionary<String, int> prefNumbers = new Dictionary<String, int>();
            foreach (var item in studentProgrammes)
            {
                var prefNumber = from pref in db.Preferences
                                 where pref.EGN == user && pref.ProgrammeName == item
                                 select pref.PrefNumber;
                prefNumbers.Add(item, prefNumber.First());
            }

            foreach (var item in studentRankList)
            {
                String faculty = db.Faculties.Find(item.ProgrammeName).FacultyName;
                StudentRankingInformation r = new StudentRankingInformation
                {
                    FacultyName = faculty,
                    FinalResult = item.TotalGrade,
                    ProgrammeName = item.ProgrammeName,
                    PrefNumber = prefNumbers[item.ProgrammeName]
                };
                model.Add(r);
            }
            ViewData["result"] = model;

            model.Add(new StudentRankingInformation { FacultyName = "FMI", FinalResult = 22.4, PrefNumber = 1, ProgrammeName = "KN" });
            return View(model);
        }

        [HttpPost]
        public ActionResult EnrollStudent(String programmeName)
        {
            String user = User.Identity.Name;
            ViewData["userName"] = user;
            Student st = db.Students.Find(user);

            st.IsEnrolled = true;
            
            db.Entry(st).State = EntityState.Modified;
            db.SaveChanges();
            ViewData["isEnrolled"] = true;
            //return RedirectToAction("Index", "StudentRankingInformation");
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "StudentRankingInformation");
            return Json(new { Url = redirectUrl });
        }

    }
}
