using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimeTest.Data.Entity;

namespace LimeTest.Data
{
    public class LimeContext : DbContext
    {
        public LimeContext() : base("DBConnection")
        {
        }

        public DbSet<People> Peoples { get; set; }
        public DbSet<Poem> Poems { get; set; }
    }
}
