using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnEnglish.Models
{
    public class TranslationCheck
    {
        public int TranslationId { get; set; }
        public string ArmTranslation { get; set; }
        public string FilledTranslation { get; set; }
    }
}