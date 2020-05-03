using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CovidBot.Models;
using Microsoft.EntityFrameworkCore;

namespace CoronaBot.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FAQIntent>()
                .HasIndex(x => x.Intent)
                .IsUnique(true);
        }
        public DbSet<SelfCertification> SelfCertifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FAQIntent> FAQIntents { get; set; }
        public DbSet<FAQQuestion> FAQQuestions { get; set; }
        public DbSet<FAQAnswer> FAQAnswers { get; set; }
        public DbSet<UserQuestion> UserQuestions { get; set; }
        public DbSet<ProvinceCovidStats> ProvinceCovidStats { get; set; }
        public DbSet<RegionCovidStats> RegionCovidStats { get; set; }
        public DbSet<FeedItem> FeedItems { get; set; }
        public DbSet<FeedRss> FeedRss { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
    }
}
