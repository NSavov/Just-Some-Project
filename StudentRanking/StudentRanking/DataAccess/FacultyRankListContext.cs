﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StudentRanking.Models
{
    public class FacultyRankListContext
    {
        public DbSet<FacultyRankList> FacultyRankLists { get; set; }
    }
}