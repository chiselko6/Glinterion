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
        private IPhotoRepository photosDb;
        private IUserRepository usersDb;

        public ImageRepository(IPhotoRepository photoRepository, IUserRepository userRepository)
        {
            photosDb = photoRepository;
            usersDb = userRepository;
        }

        public async void Save(Stream dataStream, string userLogin, string photoDescription, double rating)
        {
            var allPhotosCount = photosDb.GetPhotos().Count();
            var userId = usersDb.GetUsers().First(user => user.Login == userLogin).ID;
            int photoNumber = photosDb.GetPhotos().Count(ph => ph.UserID == userId) + 1;
            //var dataStream = await file.ReadAsStreamAsync();
            byte[] bufferOriginal = new byte[dataStream.Length];
            await dataStream.ReadAsync(bufferOriginal, 0, (int)dataStream.Length);
            var uploadFolderOriginal = "images/user_" + userLogin + "/original/";
            var uploadFolderPreview = "images/user_" + userLogin + "/preview/";
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
                ID = allPhotosCount + 1,
                UserID = userId,
                Size = (double)dataStream.Length / 1024 / 1024,
                Rating = rating,
                SrcOriginal = uploadFolderOriginal,
                SrcPreview = uploadFolderPreview
            };
            photosDb.AddPhoto(photo);
            photosDb.Save();
        }
    }
}