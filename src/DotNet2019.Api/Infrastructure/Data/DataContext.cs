using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2019.Api.Infrastructure.Data
{
    public class DataContext: DbContext
    {
        internal DbSet<User> Users { get; set; }
        public DataContext(DbContextOptions options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<User>();

            user.HasKey(u => u.Id);
            user.Property(u => u.Id).ValueGeneratedOnAdd();
            user.Property(u => u.Name).IsRequired().HasMaxLength(200);                
        }
    }
}
