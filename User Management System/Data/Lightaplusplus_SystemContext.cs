﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lightaplusplus.Models;

namespace Lightaplusplus.Data
{
    public class Lightaplusplus_SystemContext : DbContext
    {
        public Lightaplusplus_SystemContext (DbContextOptions<Lightaplusplus_SystemContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<UserLinks> UserLinks { get; set; }
        public DbSet<UserPictures> UserPictures { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Sections> Sections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasAlternateKey(u => u.email);
            modelBuilder.Entity<SectionStudents>()
                .HasKey(ss => new { ss.SectionId, ss.StudentId });
        }
    }
}
