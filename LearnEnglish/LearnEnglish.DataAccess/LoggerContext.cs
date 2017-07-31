using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnEnglish.BusinessObjects;

namespace LearnEnglish.DataAccess
{
    class LoggerContext : DbContext
    {
        public LoggerContext() : base("ExceptionConnection")
        {
        }

        public DbSet<ExceptionDetail> ExceptionDetails { get; set; }
    }
}
