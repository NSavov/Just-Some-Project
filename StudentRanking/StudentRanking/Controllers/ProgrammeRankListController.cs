using StudentRanking.DataAccess;
using StudentRanking.Ranking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentRanking.Controllers
{
    public class ProgrammeRankListController : Controller
    {
        private RankingContext db = new RankingContext();
        private Dictionary<String, List<String>> programmes = new Dictionary<String, List<String>>();
        private List<FacultyRankList> model = new List<FacultyRankList>();
        //


        public ProgrammeRankListController()
        {
            var faculties = db.Faculties.ToList();

            foreach (var faculty in faculties)
            {
                if (!programmes.ContainsKey(faculty.FacultyName))
                {
                    List<String> specialities = new List<String>();
                    programmes.Add(faculty.FacultyName, specialities);
                    //specialities.Add(faculty.ProgrammeName);
                    programmes[faculty.FacultyName].Add(faculty.ProgrammeName);
                }
                else
                {
                    programmes[faculty.FacultyName].Add(faculty.ProgrammeName);
                }
            }
        }

        public JsonResult GetProgrammes(string faculty)
        {
            if (faculty != "Please Select")
                return Json(programmes[faculty]);
            else
                return Json(new List<String>());
        }




        // GET: /ProgrammeRankList/
        [HttpGet]
        public ActionResult Index()
        {

            String user = User.Identity.Name;
            user = "Evgeny";
            ViewData["userName"] = user;

            List<String> l = programmes.Keys.ToList<string>();
            l.Insert(0, "Please Select");
            SelectList faculties = new SelectList(l);

            ViewData["faculties"] = faculties;

            ViewData["result"] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(String faculty, String programmeName)
        {
            List<String> l = programmes.Keys.ToList<string>();
            l.Insert(0, "Please Select");
            SelectList faculties = new SelectList(l);

            ViewData["faculties"] = faculties;


            QueryManager queryManager = new QueryManager(db);

            List<FacultyRankList> rankList = queryManager.getRankList(programmeName);

            foreach (var item in rankList)
            {
                FacultyRankList rank = new FacultyRankList
                {
                    ProgrammeName = programmeName,
                    EGN = item.EGN,
                    TotalGrade = item.TotalGrade
                };
                model.Add(rank);
            }

            FacultyRankList f = new FacultyRankList
            {
                EGN = "12345678",
                ProgrammeName = programmeName,
                TotalGrade = 4.5
            };
            model.Add(f);

            ViewData["result"] = model;
            
            return PartialView("_ProgrammeRankListTable", model);
        }

    }
}
