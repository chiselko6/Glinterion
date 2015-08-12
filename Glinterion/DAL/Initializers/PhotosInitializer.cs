using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Glinterion.Models;

namespace Glinterion.DAL.Initializers
{
    public class PhotosInitializer : DropCreateDatabaseIfModelChanges<PhotosContext>
    {
        protected override void Seed(PhotosContext context)
        {
            var photos = new List<Photo>
            {
                new Photo
                {
                    ID = 1,
                    Description = "just a glass ball in sunset",
                    Rating = 5.0,
                    SrcPreview = "~/images/user_chiselko6/preview/img1.jpg",
                    SrcOriginal = "~/images/user_chiselko6/original/img1.jpeg"
                },
                new Photo
                {
                    ID = 1,
                    Description = "white bears",
                    Rating = 4.9,
                    SrcPreview = "~/images/user_chiselko6/preview/img2.jpg",
                    SrcOriginal = "~/images/user_chiselko6/original/img2.jpg"
                },
                new Photo
                {
                    ID = 1,
                    Description = "envy wolves",
                    Rating = 4.8,
                    SrcPreview = "~/images/user_chiselko6/preview/img3.jpg",
                    SrcOriginal = "~/images/user_chiselko6/original/img3.jpg"
                },
                new Photo
                {
                    ID = 1,
                    Description = "pretty kitten",
                    Rating = 5.0,
                    SrcPreview = "~/images/user_chiselko6/preview/img4.jpg",
                    SrcOriginal = "~/images/user_chiselko6/original/img4.jpg"
                },
                new Photo
                {
                    ID = 1,
                    Description = "love is...",
                    Rating = 5.0,
                    SrcPreview = "~/images/user_chiselko6/preview/img5.jpg",
                    SrcOriginal = "~/images/user_chiselko6/original/img5.jpg"
                }
            };

            photos.ForEach(photo => context.Photos.Add(photo));
            context.SaveChanges();
        }
    }
}