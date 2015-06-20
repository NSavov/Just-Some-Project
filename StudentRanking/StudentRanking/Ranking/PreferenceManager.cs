using StudentRanking.DataAccess;
using StudentRanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentRanking.Ranking
{
    public class PreferenceManager
    {
        private RankingContext context;
        private QueryManager queryManager;

        public PreferenceManager(RankingContext context)
        {
            this.context = context;
            queryManager = new QueryManager(context);
        }



        public Dictionary<String, List<Preference>> splitPreferencesByFaculty(List<Preference> preferences)
        {
            Dictionary<String, List<Preference>> splittedPreferences = new Dictionary<String, List<Preference>>();
            List<Preference> value;

            foreach (Preference preference in preferences)
            {
                String faculty = queryManager.getFaculty(preference.ProgrammeName);


                if (!splittedPreferences.TryGetValue(faculty, out value))
                {
                    splittedPreferences.Add(faculty, new List<Preference>());
                }

                splittedPreferences[faculty].Add(preference);
            }

            return splittedPreferences;
        }
    }
}