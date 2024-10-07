﻿using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain;

namespace Shop.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options)
        : base(options) { }


        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
<<<<<<< HEAD
        public DbSet<RealEstate> RealEstates { get; set; }

=======
        public DbSet<Kindergarten> Kindergartens { get; set; }
>>>>>>> KinderGarten

    }
}
