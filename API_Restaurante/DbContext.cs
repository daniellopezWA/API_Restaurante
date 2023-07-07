using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Restaurante.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API_Restaurante
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        public DbSet<Mesero> Meseros { get; set; }
        public DbSet<Mesas> Mesas { get; set; }
        public DbSet<Propina> Propinas { get; set; }
    }
}
