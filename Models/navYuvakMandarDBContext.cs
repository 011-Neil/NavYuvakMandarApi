using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace NavYuvakMandarApi.Models
{
    public class navYuvakMandarDBContext:DbContext
    {
     
    

        public DbSet<User> RegisterUser { get; set; }
        public object Users { get; internal set; }
        public DbSet<AartiDetails> AartiData { get; set; }

        public object AartiDetails { get; internal set; }

        public navYuvakMandarDBContext(DbContextOptions<navYuvakMandarDBContext> options) : base(options)
        {
        }

 
    }

  
}
