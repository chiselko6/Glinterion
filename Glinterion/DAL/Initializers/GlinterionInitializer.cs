using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glinterion.Models;

namespace Glinterion.DAL.Initializers
{
    public class GlinterionInitializer : CreateDatabaseIfNotExists<GlinterionContext>
    {
        protected override void Seed(GlinterionContext context)
        {
            context = DependencyResolver.Current.GetService<GlinterionContext>();

            var roles = new List<Role>
            {
                new Role()
                {
                    RoleId = 1,
                    Name = "admin",
                    Users = new List<User>()
                },

                new Role()
                {
                    RoleId = 2,
                    Name = "user",
                    Users = new List<User>()
                }
            };

            var accounts = new List<Account>
            {
                new Account()
                {
                    AccountId = 1,
                    Name = "Base",
                    Color = Color.Green,
                    Duration = new TimeSpan(0),
                    Users = new List<User>(),
                    MaxSize = 1.0
                },

                new Account()
                {
                    AccountId = 2,
                    Name = "Gold",
                    Color = Color.Red,
                    Duration = new TimeSpan(30, 0, 0, 0),
                    Users = new List<User>(),
                    MaxSize = null
                }
            };

            var accountSerials = new List<AccountSerial>
            {
                new AccountSerial
                {
                    Account = accounts[0],
                    Serial = "123456qwerty123456"
                },

                new AccountSerial
                {
                    Account = accounts[0],
                    Serial = "qwerty123456qwerty"
                },

                new AccountSerial
                {
                    Account = accounts[1],
                    Serial = "qwertyqwertyqwerty"
                },

                new AccountSerial
                {
                    Account = accounts[1],
                    Serial = "123456123456123456"
                }
            };


            roles.ForEach(role => context.Roles.Add(role));
            accounts.ForEach(account => context.Accounts.Add(account));
            accountSerials.ForEach(a => context.AccountsSerials.Add(a));

            context.SaveChanges();

        }
    }
}