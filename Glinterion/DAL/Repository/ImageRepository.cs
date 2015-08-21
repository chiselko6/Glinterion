using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Glinterion.DAL.IRepository;
using Glinterion.Models;
using Glinterion.PhotoHelpers;

namespace Glinterion.DAL.Repository
{
    public class ImageRepository : IImageRepository
    {
        private IGenericRepository<Photo> photosDb;
        private IGenericRepository<User> usersDb;

        public ImageRepository(IUnitOfWork uof)
        {

            photosDb = uof.Repository<Photo>();
            usersDb = uof.Repository<User>();
        }

        public async void Save(Stream dataStream, User user, string photoDescription, double rating)
        {
            var allPhotosCount = photosDb.GetAll().Count();
            var userId = usersDb.GetAll().First(u => u.Login == user.Login).UserId;
            int photoNumber = photosDb.GetAll().AsEnumerable().Count(ph => ph.User.UserId == userId) + 1;
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
            string suffix = "img" + photoNumber + ".jpg";
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
                PhotoId = allPhotosCount + 1,
                User = user,
                Size = (double)dataStream.Length / 1024 / 1024,
                Rating = rating,
                SrcOriginal = uploadFolderOriginal,
                SrcPreview = uploadFolderPreview
            };
            photosDb.Add(photo);
            photosDb.Save();
        }
    }
}