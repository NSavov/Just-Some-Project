using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentRanking.Models
{
    public class StudentRankingInformationController : Controller
    {
        //
        // GET: /StudentRankingInformation/


        //I found that User works, ie. User.Identity.Name or User.IsInRole("Administrator")...
        public ActionResult Index()
        {
            String user = User.Identity.Name;
            user = "Evgeny";
            ViewData["userName"] = user;

            List<StudentRankingInformation> model = new List<StudentRankingInformation>();
            model.Add(new StudentRankingInformation { FacultyName = "FMI", FinalResult = 22.4, PrefNumber = 1, ProgrammeName = "KN" });
            return View(model);
        }

    }
}
