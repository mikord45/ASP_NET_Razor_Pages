using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace ASP_NET_Razor_Pages.Data
{
    public class ASP_NET_Razor_PagesContext : DbContext
    {
        public ASP_NET_Razor_PagesContext (DbContextOptions<ASP_NET_Razor_PagesContext> options)
            : base(options)
        {
        }

        public DbSet<RazorPagesMovie.Models.Movie> Movie { get; set; } = default!;
    }
}
