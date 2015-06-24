using StudentRanking.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentRanking.Controllers
{
    public class CampaignDatesController : Controller
    {
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
            HttpContext.Current.Application["date"] = ranking.FirstRankingDate.ToString();
            String s = ConfigurationManager.AppSettings["PublishFirstRankingDate"];
            ConfigurationManager.AppSettings["PublishFirstRankingDate"] = ranking.FirstRankingDate.ToString();
            ConfigurationManager.AppSettings["PublishSecondRankingDate"] = ranking.SecondRankingDate.ToString();
            ConfigurationManager.AppSettings["PublishThirdRankingDate"] = ranking.ThirdRankingDate.ToString();
            ConfigurationManager.AppSettings["AddingPreferencesFirstDate"] = ranking.StudentPreferenceFirstDate.ToString();
            ConfigurationManager.AppSettings["AddingPreferencesLastDate"] = ranking.StudentPreferenceLastDate.ToString();
            //String s = ConfigurationManager.AppSettings["FirstRankingDate"];
            //DateTime t = Convert.ToDateTime(ConfigurationManager.AppSettings["PublishFirstRankingDate"]);

            return View(ranking);
        }
    }
}
