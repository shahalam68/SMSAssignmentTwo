using Microsoft.EntityFrameworkCore;
using SMSDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudenMangementSystem.Data.Data
{
    public class StudentAPIDbContext : DbContext
    {
        public StudentAPIDbContext(DbContextOptions<StudentAPIDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=admin;Database=student");
            }
        }
    }
}
