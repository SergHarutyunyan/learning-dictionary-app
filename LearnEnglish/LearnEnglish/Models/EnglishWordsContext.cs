using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LearnEnglish.Models
{
    public class EnglishWordsContext : DbContext
    {
        public DbSet<EnglishWords> EngWords { get; set; }
    }
}