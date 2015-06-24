using StudentRanking.DataAccess;
using StudentRanking.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentRanking.Controllers
{
    public class CampaignRankingDatesController : Controller
    {
        private RankingContext db = new RankingContext();
        //
        // GET: /RankingDates/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CampaignRankingDates ranking)
        {
            
            RankingDates dates = new RankingDates
            {
                FirstRankingDate = ranking.FirstRankingDate.ToString(),
                PreferrencesFirstDate = ranking.StudentPreferenceFirstDate.ToString(),
                PreferrencesLastDate = ranking.StudentPreferenceLastDate.ToString(),
                SecondRankingDate = ranking.SecondRankingDate.ToString(),
                ThirdRankingDate = ranking.ThirdRankingDate.ToString()
            };
            if (db.Dates.ToList().Count() == 0)
            {
                
                db.Dates.Add(dates);
                db.SaveChanges();
            }
            else
            {
                RankingDates p = db.Dates.ToList().First();
                db.Dates.Attach(p);
                db.Dates.Remove(p);
                db.SaveChanges();

                db.Dates.Add(dates);
                db.SaveChanges();
            }


            return RedirectToAction("Menu", "Admin");  
        }
    }
}
