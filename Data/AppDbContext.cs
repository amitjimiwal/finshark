using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stockapi.Models;

namespace stockapi.Data
{
    public class AppDbContext : DbContext
    {
        // AppDbContext inherits the DBContext class and here we are passing the options parameter to the super class using the `base` which call constructor of parent class
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        //The database reference we wrap it in the DbContext
        public DbSet<Stock> Stocks{get; set;}
        public DbSet<Comment> Comments{get; set;}
    }
}