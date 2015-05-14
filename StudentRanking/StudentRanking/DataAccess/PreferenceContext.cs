using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StudentRanking.Models
{
    public class PreferenceContext : DbContext
    {
        public DbSet<Preference> Preferences { get; set; }
    }
}