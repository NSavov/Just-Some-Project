using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentRanking.Models;
using StudentRanking.DataAccess;
using WebMatrix.WebData;
using System.Web.Security;
using StudentRanking.Ranking;

namespace StudentRanking.Controllers
{
    public class StudentBrowseController : Controller
    {
        private RankingContext db = new RankingContext();

        //
        // GET: /StudentBrowse/

        public ActionResult Index(String egn = "")
        {

            Ranker ranker = new Ranker(db);
            ranker.test();


            if (egn == "")
                return View(db.Students.ToList());
            else
                return Details(egn);
        }

        //
        // GET: /StudentBrowse/

        //[HttpPost]
        //public ActionResult Index(String egn)
        //{

        //    return Details(egn);
        //}


        //
        // GET: /StudentBrowse/Details/5

        public ActionResult Details(string id = null)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // GET: /StudentBrowse/Create

        public ActionResult Create()
        {
            ViewBag.Password = Membership.GeneratePassword(10, 0);
            return View();
        }

        //
        // POST: /StudentBrowse/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                
                db.Students.Add(student);
                db.SaveChanges();

                WebSecurity.CreateUserAndAccount(student.EGN, ViewBag.Password);
                WebSecurity.Login(student.EGN, ViewBag.Password);

                MembershipUser user = Membership.GetUser(student.EGN);
                user.GetPassword();

                var roles = (SimpleRoleProvider)Roles.Provider;

                if (!roles.RoleExists("admin"))
                    roles.CreateRole("admin");

                if (!roles.RoleExists("student"))
                    roles.CreateRole("student");

                roles.AddUsersToRoles(new string[] { student.EGN }, new string[] { "student" });
            }

            return View(student);
        }

        //
        // GET: /StudentBrowse/Edit/5

        public ActionResult Edit(string id = null)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // POST: /StudentBrowse/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        //
        // GET: /StudentBrowse/Delete/5

        public ActionResult Delete(string id = null)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // POST: /StudentBrowse/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}