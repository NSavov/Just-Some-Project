using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class ProgrammeRules
    {
        [Required]
        [Key]
        public String ProgrammeName{ get; set; }

        [Required]
        public ushort MaleCount{ get; set; }

        [Required]
        public ushort FemaleCount{ get; set; }
    }
}