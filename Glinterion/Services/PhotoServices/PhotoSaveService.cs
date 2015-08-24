using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Glinterion.DAL.IRepository;
using Glinterion.Models;
using Glinterion.PhotoHelpers;

namespace Glinterion.Services.PhotoServices
{
    public class PhotoSaveService
    {
        private IGenericRepository<Photo> photosDb;
        private IGenericRepository<User> usersDb;

        public PhotoSaveService(IUnitOfWork uof)
        {

            photosDb = uof.Repository<Photo>();
            usersDb = uof.Repository<User>();
        }

        public async void Save(Stream dataStream, User user, string photoDescription, double rating, string fileExtension)
        {
            //var allPhotosCount = photosDb.GetAll().Count();
            var userId = user.UserId;
            
            //int photoNumber = photosDb.GetAll().AsEnumerable().Count(ph => ph.User.UserId == userId) + 1;
            //var dataStream = await file.ReadAsStreamAsync();

            byte[] bufferOriginal = new byte[dataStream.Length];
            await dataStream.ReadAsync(bufferOriginal, 0, (int)dataStream.Length);
            var uploadFolderOriginal = "images/user_" + user.Login + "/original/";
            var uploadFolderPreview = "images/user_" + user.Login + "/preview/";
            var rootOriginal = HttpContext.Current.Server.MapPath("~/" + uploadFolderOriginal);
            var rootPreview = HttpContext.Current.Server.MapPath("~/" + uploadFolderPreview);
            Directory.CreateDirectory(rootOriginal);
            Directory.CreateDirectory(rootPreview);

            byte[] bufferPreview = PhotoConverter.Resize(bufferOriginal, 100, 100);

            // TODO: get file extension
            string suffix = Guid.NewGuid() + "." + fileExtension;
            rootOriginal +=  suffix;
            uploadFolderOriginal += suffix;
            rootPreview += suffix;
            uploadFolderPreview += suffix;
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
                User = user,
                Size = (double)dataStream.Length / 1024 / 1024,
                Rating = rating,
                SrcOriginal = uploadFolderOriginal,
                SrcPreview = uploadFolderPreview,
                //UserId = user.UserId,
                //PhotoId = 1

            };
            photosDb.Add(photo);
            photosDb.Save();
        }
    }
}