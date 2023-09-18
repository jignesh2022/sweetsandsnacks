﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SweetsAndSnacks.Models;

namespace SweetsAndSnacks.Data
{
    public class DBContext:IdentityDbContext
    {
        public DBContext(DbContextOptions<DBContext> options):base(options)
        {

        }

        public DbSet<Product> Products { get; set; }        
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<SweetsAndSnacks.Models.CartItemDto>? CartItemDto { get; set; }
    }
}
