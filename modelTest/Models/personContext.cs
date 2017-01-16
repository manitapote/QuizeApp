using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using modelTest.Models;

namespace modelTest.Models
{
    public class personContext : DbContext
    {        
        public DbSet<person> personss { get; set; }
        public DbSet<questionGRE> questionsGRE { get; set; }
        public DbSet<questionSAT> questionsSAT { get; set; }
    }
}
