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
        private List<ProgrammeRules> model = new List<ProgrammeRules>();

        private Dictionary<String, List<String>> programmes = new Dictionary<String, List<String>>(); 
        //
        // GET: /ProgrammeRules/
        public ProgrammeRulesController() : base()
        {
            List<Faculty> faculties = new List<Faculty>();
            faculties = db.Faculties.ToList();

            foreach(var faculty in faculties)
            {
                if (!programmes.ContainsKey(faculty.FacultyName))
                {
                    List<String> specialities = new List<String>();
                    programmes.Add(faculty.FacultyName, specialities);
                    specialities.Add(faculty.ProgrammeName);
                }
                else
                {
                    programmes[faculty.FacultyName].Add(faculty.ProgrammeName);
                }
            }
            programmes.Add("USA", new List<String>());
            programmes.Add("UK", new List<String>());
            programmes.Add("India", new List<String>());

        }

        public JsonResult GetStates(string country)
        {
            List<string> states = new List<string>();
            switch (country)
            {
                case "USA":
                    states.Add("California");
                    states.Add("Florida");
                    states.Add("Ohio");
                    break;
                case "UK":
                    //add UK states here
                    states.Add("Manchester");
                    states.Add("Leeds");
                    states.Add("London");
                    break;
                case "India":
                    //add India states hete
                    states.Add("Mumbai");
                    states.Add("New Delhi");

                    break;
            }
            return Json(states);
        }
        [HttpPost]
        public ActionResult Index(string country,string state)
        {
            ModelState.Clear();
            ProgrammeRules rules1 = new ProgrammeRules { FemaleCount = 50, MaleCount = 100, ProgrammeName = "KN" };

            ProgrammeRules rules2 = new ProgrammeRules { FemaleCount = 50, MaleCount = 100, ProgrammeName = "IS" };

            ProgrammeRules rules3 = new ProgrammeRules { FemaleCount = 50, MaleCount = 100, ProgrammeName = "Softuerno" };
            //List<ProgrammeRules> rules = new List<ProgrammeRules>();
            model.Add(rules1);
            model.Add(rules2);
            model.Add(rules3);

            //db.ProgrammesRules.Add(rules1);
            //db.ProgrammesRules.Add(rules2);
            //db.ProgrammesRules.Add(rules3);
            //db.SaveChanges();

            SelectList countries = new SelectList(programmes.Keys.ToList<string>());
            ViewData["countries"] = countries;

            //UserData newmodel = new UserData();
            //ModelState.Clear();
            //model = newmodel;

            

            return View(model);
        }

        public ActionResult Index()
        {
            SelectList countries = new SelectList(programmes.Keys.ToList<string>());
            ViewData["countries"] = countries;
            ViewBag.Programmes = programmes;
            return View(model );
        }

        //
        // GET: /ProgrammeRules/Details/5

        public ActionResult Details(string id = null)
        {
            ProgrammeRules programmerules = db.ProgrammesRules.Find(id);
            if (programmerules == null)
            {
                return HttpNotFound();
            }
            return View(programmerules);
        }

        //
        // GET: /ProgrammeRules/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProgrammeRules/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProgrammeRules programmerules)
        {
            if (ModelState.IsValid)
            {
                db.ProgrammesRules.Add(programmerules);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programmerules);
        }

        //
        // GET: /ProgrammeRules/Edit/5

        public ActionResult Edit(string id = null)
        {
            ProgrammeRules programmerules = db.ProgrammesRules.Find(id);
            if (programmerules == null)
            {
                return HttpNotFound();
            }
            return View(programmerules);
        }

        //
        // POST: /ProgrammeRules/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProgrammeRules programmerules)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programmerules).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(programmerules);
        }

        //
        // GET: /ProgrammeRules/Delete/5

        public ActionResult Delete(string id = null)
        {
            ProgrammeRules programmerules = db.ProgrammesRules.Find(id);
            if (programmerules == null)
            {
                return HttpNotFound();
            }
            return View(programmerules);
        }

        //
        // POST: /ProgrammeRules/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ProgrammeRules programmerules = db.ProgrammesRules.Find(id);
            db.ProgrammesRules.Remove(programmerules);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult AsyncOrders(string title)
        {
            Dictionary<String, List<String>> dict = new Dictionary<String, List<String>>();
            dict["Ana Trujillo"] = new List<String>();
            dict["Ana Trujillo"].Add("1");
            dict["Ana Trujillo"].Add("2"); dict["Ana Trujillo"].Add("3");
            //faculty = "Ana Trujillo"; 
            var specialities = dict[title].Select( a => new SelectListItem()
            //var specialities = programmes[faculty].Select( a => new SelectListItem()

            {

                //Text = a.OrderDate.HasValue ? a.OrderDate.Value.ToString("MM/dd/yyyy") : "[ No Date ]",

                //Value = a.OrderID.ToString(),
                Text = a,
                Value = dict[title].IndexOf(a).ToString()

            });

            //return Json(orders);
            //var Orders = new List<String>();

            return Json(specialities);

        }
    }
}