﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Glinterion.DAL.IRepository;
using Glinterion.Models;

namespace Glinterion.DAL.Repository
{
    public class ImageRepository : IImageRepository
    {
        private IPhotoRepository photosDb;
        private IUserRepository usersDb;

        public ImageRepository(IPhotoRepository photoRepository, IUserRepository userRepository)
        {
            photosDb = photoRepository;
            usersDb = userRepository;
        }

        public async void Save(HttpContent file, string userLogin, string photoDescription, double rating)
        {
            var allPhotosCount = photosDb.GetPhotos().Count();
            var userId = usersDb.GetUsers().First(user => user.Login == userLogin).ID;
            int photoNumber = photosDb.GetPhotos().Count(ph => ph.UserID == userId) + 1;
            var dataStream = await file.ReadAsStreamAsync();
            byte[] bufferOriginal = new byte[dataStream.Length];
            await dataStream.ReadAsync(bufferOriginal, 0, (int)dataStream.Length);
            var uploadFolderOriginal = "~/images/user_" + userLogin + "/original/";
            var uploadFolderPreview = "~/images/user_" + userLogin + "/preview/";
            var rootOriginal = HttpContext.Current.Server.MapPath(uploadFolderOriginal);
            var rootPreview = HttpContext.Current.Server.MapPath(uploadFolderPreview);
            Directory.CreateDirectory(rootOriginal);
            Directory.CreateDirectory(rootPreview);

            // TODO: convert to a smaller size
            byte[] bufferPreview = bufferOriginal;

            // TODO: get file extension
            rootOriginal += "img" + photoNumber + ".jpg";
            rootPreview += "img" + photoNumber + ".jpg";
            using (var stream = new FileStream(rootOriginal, FileMode.OpenOrCreate))
            {
                await stream.WriteAsync(bufferOriginal, 0, bufferOriginal.Length);
            }
            using (var stream = new FileStream(rootPreview, FileMode.OpenOrCreate))
            {
                await stream.WriteAsync(bufferPreview, 0, bufferPreview.Length);
            }

            var photo = new Photo
            {
                Description = photoDescription,
                ID = allPhotosCount + 1,
                UserID = userId,
                Size = (double)dataStream.Length / 1000000,
                Rating = rating,
                SrcOriginal = rootOriginal,
                SrcPreview = rootPreview
            };
            photosDb.AddPhoto(photo);
            photosDb.Save();
        }
    }
}