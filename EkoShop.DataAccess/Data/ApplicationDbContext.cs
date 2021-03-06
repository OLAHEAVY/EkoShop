﻿using System;
using System.Collections.Generic;
using System.Text;
using EkoShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EkoShop.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Brand> Brand { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Coupon> Coupon { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Post> Post { get; set; }
    }
}
