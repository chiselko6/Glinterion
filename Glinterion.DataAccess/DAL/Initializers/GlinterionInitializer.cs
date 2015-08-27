using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                    Name = "admin",
                    Users = new List<User>()
                },

                new Role()
                {
                    Name = "user",
                    Users = new List<User>()
                },

                new Role()
                {
                    Name = "moderator",
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
                    MaxSize = 999
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

            //accounts[0].Serials.Add(accountSerials[0]);
            //accounts[0].Serials.Add(accountSerials[1]);
            //accounts[1].Serials.Add(accountSerials[2]);
            //accounts[1].Serials.Add(accountSerials[3]);

            var users = new List<User>
            {
                new User
                {
                    Account = accounts[1],
                    Email = "admin@glinterion.com",
                    FirstName = "admin",
                    LastName = "admin",
                    Login = "admin",
                    Password = GetMD5Hash("GCDuserHashADMIN"),
                    Role = roles[0],
                    Photos = new List<Photo>()
                },

                new User
                {
                    Account = accounts[1],
                    Email = "moderator@glinterion.com",
                    FirstName = "moderator",
                    LastName = "moderator",
                    Login = "moderator",
                    Password = GetMD5Hash("GCDuserHashMODERATOR"),
                    Role = roles[2],
                    Photos = new List<Photo>()
                }
            };

            roles.ForEach(role => context.Roles.Add(role));
            accounts.ForEach(account => context.Accounts.Add(account));
            accountSerials.ForEach(a => context.AccountsSerials.Add(a));
            users.ForEach(user => context.Users.Add(user));

            context.SaveChanges();

        }

        private string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        
    }
}