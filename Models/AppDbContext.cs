using System;
using Microsoft.EntityFrameworkCore;

namespace CoronaBot.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<SelfCertification> SelfCertifications { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
