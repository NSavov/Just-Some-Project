using StudentRanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace StudentRanking.Controllers
{
    public class StudentPreferencesController : Controller
    {
        //
        // GET: /StudentPreferences/
        private Dictionary<String, List<String>> programmes = new Dictionary<String, List<String>>();
        private List<StudentPreferences> model = new List<StudentPreferences>();

        public StudentPreferencesController()
        {
            List<String> p1 = new List<String>();
            p1.Add("KN");
            p1.Add("Info");
            p1.Add("IS");

            List<String> p2 = new List<String>();
            p2.Add("ikonomika");
            p2.Add("Selsko stopanstvo");

            List<String> p3 = new List<String>();
            p3.Add("Biologiq");
            p3.Add("Biotehnologii");
            p3.Add("Molekulqrna");

            List<String> faculties = new List<String>();
            faculties.Add("FMI");
            faculties.Add("Stopanski");
            faculties.Add("Bilogicheski");

            programmes.Add(faculties[0], p1);
            programmes.Add(faculties[1], p2);
            programmes.Add(faculties[2], p3);

        }

        public JsonResult GetProgrammes(string faculty)
        {
            if (faculty != "Please Select")
                return Json(programmes[faculty]);
            else
                return Json(new List<String>());
        }

        [HttpGet]
        public ActionResult Index()
        {
            String user = User.Identity.Name;
            user = "Evgeny";
            ViewData["userName"] = user;

            List<String> l = programmes.Keys.ToList<string>();
            l.Insert(0,"Please Select");
            SelectList faculties = new SelectList(l);
       
            ViewData["faculties"] = faculties;
            ViewData["result"] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(String faculty, String programmeName)
        {
            String user = User.Identity.Name;
            user = "Evgeny";
            ViewData["userName"] = user;

            List<String> l = programmes.Keys.ToList<string>();
            l.Insert(0, "Please Select");
            SelectList faculties = new SelectList(l);

            ViewData["faculties"] = faculties;

            StudentPreferences pref = new StudentPreferences { Faculty = faculty, ProgrammeName = programmeName, PrefNumber = 1 };
            model.Add(pref);
            ModelState.Clear();
            ViewData["result"] = model;

            return PartialView("_StudentPreferencesTable", model);
            //return View("Index",model);
        }


    }
}
