using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Glinterion.DAL.Contexts;
using Glinterion.Models;

namespace Glinterion.DAL.Initializers
{
    public class UsersInitializer : DropCreateDatabaseAlways<UsersContext>
    {
        protected override void Seed(UsersContext context)
        {
            var users = new List<User>
            {
                new User()
                {
                    ID = 1,
                    Login = "chiselko6",
                    Role = "admin"
                },

                new User()
                {
                    ID = 2,
                    Login = "RomanFrom710",
                    Role = "user"
                }
            };

            users.ForEach(user => context.Users.Add(user));
            context.SaveChanges();
        }
    }
}