using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LearnEnglish.Models
{
    [Table("EnglishWords")]
    public class EnglishWords
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string armTranslation { get; set; }
        public bool isLearned { get; set; }

    }
}