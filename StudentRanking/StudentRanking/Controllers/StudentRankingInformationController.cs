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
            return View();
        }

    }
}
