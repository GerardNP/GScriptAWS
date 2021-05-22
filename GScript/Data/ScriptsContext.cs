using GScriptNuget;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GScript.Data
{
    public class ScriptsContext : DbContext
    {
        public ScriptsContext(DbContextOptions<ScriptsContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
