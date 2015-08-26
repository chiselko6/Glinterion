using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Glinterion.Models;

namespace Glinterion.DAL
{
    public class GlinterionContext : DbContext
    {
        public GlinterionContext()
            : base("GlinterionContext")
        {
        }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<AccountSerial> AccountsSerials { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.Photos)
            //    .WithRequired(u => u.User);
            //modelBuilder.Entity<Photo>()
            //    .HasRequired(p => p.User);
            //modelBuilder.Entity<Account>()
            //    .HasMany(a => a.Users)
            //    .WithRequired(u => u.Account);
            //modelBuilder.Entity<Account>()
            //    .HasMany(a => a.Serials)
            //    .WithRequired(s => s.Account);
            //modelBuilder.Entity<Role>()
            //    .HasMany(r => r.Users)
            //    .WithRequired(u => u.Role);

            // when database is being created, its name is going to be 'Photos',
            // so in order to make it 'Photo' according to db name conventions we 
            // have to use this approach

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 
        }
    }
}