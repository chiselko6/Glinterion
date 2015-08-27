using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Glinterion.DAL.IRepository;
using Glinterion.Models;
using Glinterion.PhotoHelpers;
//using Ninject.Activation;

namespace Glinterion.Services.PhotoServices
{
    public class PhotoSaveService
    {
        private IGenericRepository<Photo> photosDb;
        private IGenericRepository<User> usersDb;

        private IGenericRepository<Account> accountsDb; 

        public PhotoSaveService(IUnitOfWork uof)
        {

            photosDb = uof.Repository<Photo>();
            usersDb = uof.Repository<User>();
            accountsDb = uof.Repository<Account>();
        }

        public async Task<HttpResponseMessage> Save(Stream dataStream, User user, string photoDescription, double? rating, string fileExtension, bool isAvatar)
        {

            var bufferOriginal = new byte[dataStream.Length];
            var maxSize = user.Account.MaxSize;
            var photos = user.Photos;
            var currentSize = (photos == null ? 0 : photos.Sum(p => p.Size));
            // counting in MB
            if (maxSize != null &&  currentSize + (double) bufferOriginal.Length / 1024 / 1024 > maxSize)
            {
                var response = new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
                response.Content = new StringContent("The file doesn't meet size limit requirements", Encoding.UTF8, "text/plain");
                return response;
            };
            await dataStream.ReadAsync(bufferOriginal, 0, (int)dataStream.Length);
            //var prefix = System.Configuration. .ConfigurationManager.AppSettings["pathSave"] ?? "~/images/";
            var prefix = "~/images/";
            var uploadFolderOriginal = prefix + user.Login + "/original/";
            var uploadFolderPreview = prefix + user.Login + "/preview/";
            var rootOriginal = HttpContext.Current.Server.MapPath(uploadFolderOriginal);
            var rootPreview = HttpContext.Current.Server.MapPath(uploadFolderPreview);
            Directory.CreateDirectory(rootOriginal);
            Directory.CreateDirectory(rootPreview);

            var bufferPreview = PhotoConverter.Resize(bufferOriginal, 100, 100);

            var suffix = Guid.NewGuid() + "." + fileExtension;
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
            };
            photosDb.Add(photo);
            photosDb.Save();
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent("Successful upload", Encoding.UTF8, "text/plain");
            return result;
        }
    }
}