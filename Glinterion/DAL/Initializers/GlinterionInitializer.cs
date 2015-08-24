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
                    Users = new List<User>()
                },

                new Account()
                {
                    AccountId = 2,
                    Name = "Gold",
                    Color = Color.Red,
                    Duration = new TimeSpan(30, 0, 0, 0),
                    Users = new List<User>()
                }
            };


            //roles[0].Users.Add(users[0]);
            //roles[1].Users.Add(users[1]);

            //string src = @"images\";
                
            //var photos = new List<Photo>
            //{
            //    new Photo
            //    {
            //        PhotoId = 1,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "just a glass ball in sunset",
            //        Rating = 5.0,
            //        SrcPreview = src + @"user_chiselko6/preview/img1.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img1.jpeg",
            //        Size = 0.141
            //    },
            //    new Photo
            //    {
            //        PhotoId = 2,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "white bears",
            //        Rating = 4.9,
            //        SrcPreview = src + @"user_chiselko6/preview/img2.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img2.jpg",
            //        Size = 0.0492
            //    },
            //    new Photo
            //    {
            //        PhotoId = 3,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "envy wolves",
            //        Rating = 4.8,
            //        SrcPreview = src +  @"user_chiselko6/preview/img3.jpg",
            //        SrcOriginal = src +  @"user_chiselko6/original/img3.jpg",
            //        Size = 0.0608
            //    },
            //    new Photo
            //    {
            //        PhotoId = 4,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "pretty kitten",
            //        Rating = 5.0,
            //        SrcPreview = src + @"user_chiselko6/preview/img4.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img4.jpg",
            //        Size = 0.532
            //    },
            //    new Photo
            //    {
            //        PhotoId = 5,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "love is...",
            //        Rating = 5.0,
            //        SrcPreview = src + @"user_chiselko6/preview/img5.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img5.jpg",
            //        Size = 0.0608
            //    },
            //    new Photo
            //    {
            //        PhotoId = 6,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "just eye",
            //        Rating = 5.0,
            //        SrcPreview = src + @"user_chiselko6/preview/img6.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img6.jpg",
            //        Size = 4.54
            //    },
            //    new Photo
            //    {
            //        PhotoId = 7,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "pretty kittens",
            //        Rating = 5.0,
            //        SrcPreview = src + @"user_chiselko6/preview/img7.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img7.jpg",
            //        Size = 0.0349
            //    },
            //    new Photo
            //    {
            //        PhotoId = 8,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "unbelivable rose",
            //        Rating = 5.0,
            //        SrcPreview = src + @"user_chiselko6/preview/img8.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img8.jpg",
            //        Size = 0.193
            //    },
            //    new Photo
            //    {
            //        PhotoId = 9,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "Abraham Lincoln",
            //        Rating = 5.0,
            //        SrcPreview = src + @"user_chiselko6/preview/img9.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img9.jpg",
            //        Size = 0.0321
            //    },
            //    new Photo
            //    {
            //        PhotoId = 10,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "'moonset'",
            //        Rating = 5.0,
            //        SrcPreview = src + @"user_chiselko6/preview/img10.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img10.jpg",
            //        Size = 0.0313
            //    },
            //    new Photo
            //    {
            //        PhotoId = 11,
            //        User = users[0],
            //        UserId = 1,
            //        Description = "pretty monkey",
            //        Rating = 5.0,
            //        SrcPreview = src + @"user_chiselko6/preview/img11.jpg",
            //        SrcOriginal = src + @"user_chiselko6/original/img11.jpg",
            //        Size = 0.0375
            //    },

            //};

            //photos.ForEach(photo => users[0].Photos.Add(photo));

            roles.ForEach(role => context.Roles.Add(role));
            accounts.ForEach(account => context.Accounts.Add(account));
            //users.ForEach(user => context.Users.Add(user));
            //photos.ForEach(photo => context.Photos.Add(photo));

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            
        }
    }
}