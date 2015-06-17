using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentRanking.Models;
using StudentRanking.DataAccess;

namespace StudentRanking.Controllers
{
    public class ProgrammeRulesController : Controller
    {
        private RankingContext db = new RankingContext();
        private List<ProgrammeProperties> model = new List<ProgrammeProperties>();
        private Dictionary<String, List<String>> programmes = new Dictionary<String, List<String>>();

        //
        // GET: /ProgrammeRules/
        public ProgrammeRulesController() : base()
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

        [HttpPost]
        public ActionResult Index(string country,string state)
        {


            List<String> l = programmes.Keys.ToList<string>();
            l.Insert(0, "Please Select");
            SelectList faculties = new SelectList(l);

            ViewData["faculties"] = faculties;

            model.Add(new ProgrammeProperties
            {
                ProgrammeName = "KN",
                C1 = 3,
                C2 = 10,
                C3 = 9,
                C4 = 8,
                X = "math",
                Y = "Himiq",
                Z = "Biologiq",
                W = "Medicina",
                FemaleCount = 40,
                MaleCount = 100
            });
            return PartialView("_ProgrammePropertiesTable",model);
        }

        public ActionResult Index()
        {

            List<String> l = programmes.Keys.ToList<string>();
            l.Insert(0, "Please Select");
            SelectList faculties = new SelectList(l);

            ViewData["faculties"] = faculties;

            
            return View(model );
        }

        [HttpPost]
        public void SaveCounts(int maleCount, int femaleCount)
        {
            //ako crashnat int-ovete napravi si go sys string parametri
            int a = 1;
            int b = 2;
            int c = a + b;
        }
    }
}