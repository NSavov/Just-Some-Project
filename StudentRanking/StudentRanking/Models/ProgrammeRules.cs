using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class ProgrammeRules
    {
        [Key]
        public String ProgrammeName{ get; set; }
        public ushort MaleCount{ get; set; }
        public ushort FemaleCount{ get; set; }
    }
}