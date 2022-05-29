﻿using Microsoft.EntityFrameworkCore;

namespace TeamContactTelegramBot.Data.Users
{
    public class UsersContext : DbContext
    {
        public virtual DbSet<Users> DbUsers { get; set; }

        public UsersContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-N0A0TTI\SQLEXPRESS;Initial Catalog=TeamContact_001;Integrated Security=True");
        }
    }
}
