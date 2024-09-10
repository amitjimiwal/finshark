using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using stockapi.Models;

namespace stockapi.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        // AppDbContext inherits the DBContext class and here we are passing the options parameter to the super class using the `base` which call constructor of parent class
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        //The database reference we wrap it in the DbContext
        public DbSet<Stock> Stocks{get; set;}
        public DbSet<Comment> Comments{get; set;}

         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}