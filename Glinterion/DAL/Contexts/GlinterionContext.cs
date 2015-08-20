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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            // when database is being created, its name is going to be 'Photos',
            // so in order to make it 'Photo' according to db name conventions we 
            // have to use this approach
            
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 
        }
    }
}