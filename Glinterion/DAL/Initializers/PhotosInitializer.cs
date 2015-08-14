using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Glinterion.Models;

namespace Glinterion.DAL.Initializers
{
    public class PhotosInitializer : DropCreateDatabaseAlways<PhotosContext>
    {
        protected override void Seed(PhotosContext context)
        {
            string src = @"images/";
            string t = Path.GetFullPath(src);
                
            var photos = new List<Photo>
            {
                new Photo
                {
                    ID = 1,
                    Description = "just a glass ball in sunset",
                    Rating = 5.0,
                    SrcPreview = src + @"user_chiselko6/preview/img1.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img1.jpeg",
                    Size = 0.141
                },
                new Photo
                {
                    ID = 2,
                    Description = "white bears",
                    Rating = 4.9,
                    SrcPreview = src + @"user_chiselko6/preview/img2.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img2.jpg",
                    Size = 0.0492
                },
                new Photo
                {
                    ID = 3,
                    Description = "envy wolves",
                    Rating = 4.8,
                    SrcPreview = src +  @"user_chiselko6/preview/img3.jpg",
                    SrcOriginal = src +  @"user_chiselko6/original/img3.jpg",
                    Size = 0.0608
                },
                new Photo
                {
                    ID = 4,
                    Description = "pretty kitten",
                    Rating = 5.0,
                    SrcPreview = src + @"user_chiselko6/preview/img4.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img4.jpg",
                    Size = 0.532
                },
                new Photo
                {
                    ID = 5,
                    Description = "love is...",
                    Rating = 5.0,
                    SrcPreview = src + @"user_chiselko6/preview/img5.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img5.jpg",
                    Size = 0.0608
                },
                new Photo
                {
                    ID = 6,
                    Description = "just eye",
                    Rating = 5.0,
                    SrcPreview = src + @"user_chiselko6/preview/img6.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img6.jpg",
                    Size = 4.54
                },
                new Photo
                {
                    ID = 7,
                    Description = "pretty kittens",
                    Rating = 5.0,
                    SrcPreview = src + @"user_chiselko6/preview/img7.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img7.jpg",
                    Size = 0.0349
                },
                new Photo
                {
                    ID = 8,
                    Description = "unbelivable rose",
                    Rating = 5.0,
                    SrcPreview = src + @"user_chiselko6/preview/img8.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img8.jpg",
                    Size = 0.193
                },
                new Photo
                {
                    ID = 9,
                    Description = "Abraham Lincoln",
                    Rating = 5.0,
                    SrcPreview = src + @"user_chiselko6/preview/img9.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img9.jpg",
                    Size = 0.0321
                },
                new Photo
                {
                    ID = 10,
                    Description = "'moonset'",
                    Rating = 5.0,
                    SrcPreview = src + @"user_chiselko6/preview/img10.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img10.jpg",
                    Size = 0.0313
                },
                new Photo
                {
                    ID = 11,
                    Description = "pretty monkey",
                    Rating = 5.0,
                    SrcPreview = src + @"user_chiselko6/preview/img11.jpg",
                    SrcOriginal = src + @"user_chiselko6/original/img11.jpg",
                    Size = 0.0375
                },

            };

            photos.ForEach(photo => context.Photos.Add(photo));
            context.SaveChanges();
        }
    }
}