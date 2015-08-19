using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
using Glinterion.DAL;
using Glinterion.DAL.Contexts;
using Glinterion.DAL.IRepository;
using Glinterion.DAL.Repository;
using Glinterion.Models;
using Glinterion.PhotoHelpers;
using Newtonsoft.Json;

namespace Glinterion.Controllers
{
    public class PhotosController : ApiController
    {
        private IPhotoRepository photosDb;
        private IUserRepository usersDb;
        private IImageRepository imageRepository;

        public PhotosController(IPhotoRepository photoRepository, IUserRepository userRepository, IImageRepository imageRepository)
        {
            photosDb = photoRepository;
            usersDb = userRepository;
            this.imageRepository = imageRepository;
        }

        // GET: api/Photos
        public IQueryable<Photo> GetPhotos()
        {
            return photosDb.GetPhotos();
        }

        // GET: api/Photos/
        //public IQueryable<Photo> GetPhotos(int startId, int endId)
        //{
        //    if (endId < startId)
        //        return null;
        //    var photos = photosDb.GetPhotos();
        //    if (photos.Count() < startId)
        //        return null;
        //    return photos.OrderBy(photo => photo.ID).Skip(startId - 1).Take(endId - startId + 1);
        //}

        // GET: api/Photos
        public IQueryable<Photo> GetPhotos(int pageNumber, int photosPerPage)
        {
            var photos = photosDb.GetPhotos().OrderBy(photo => photo.ID);
            if (photos.Count() >= pageNumber*photosPerPage)
            {
                return photos.Skip((pageNumber - 1)*photosPerPage).Take(photosPerPage);
            }
            if (photos.Count() > (pageNumber - 1)*photosPerPage)
            {
                return photos.Skip((pageNumber - 1)*photosPerPage).Take(photos.Count() - (pageNumber - 1)*photosPerPage);
            }
            return null;
        }

            // GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        public IHttpActionResult GetPhoto(int id)
        {
            Photo photo = photosDb.GetPhotoById(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        // PUT: api/Photos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPhoto(int id, Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photo.ID)
            {
                return BadRequest();
            }

            photosDb.UpdatePhoto(photo);

            try
            {
                photosDb.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]// This is from System.Web.Http, and not from System.Web.Mvc
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var userLogin = "RomanFrom710";
            try
            {
                string description = await provider.Contents[0].ReadAsStringAsync();
                double rating = Double.Parse(await provider.Contents[1].ReadAsStringAsync());
                imageRepository.Save(provider.Contents[2], userLogin, description, rating);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent("Successful upload", Encoding.UTF8, "text/plain");
                response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(@"text/html");
                return response;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // You could extract these two private methods to a separate utility class since
        // they do not really belong to a controller class but that is up to you
        private MultipartFormDataStreamProvider GetMultipartProvider(string folder)
        {
            var user = "gcd";
            var uploadFolderOriginal = "~/images/user_" + user + "/original/"; // you could put this to web.config
            var uploadFolderPreview = "~/images/user_" + user + "/preview/"; // you could put this to web.config
            var root = HttpContext.Current.Server.MapPath(folder);
            //var rootPreview = HttpContext.Current.Server.MapPath(uploadFolderPreview);
            Directory.CreateDirectory(root);
            //Directory.CreateDirectory(rootPreview);
            return new MultipartFormDataStreamProvider(root);
        }

        // Extracts Request FormatData as a strongly typed model
        private object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData.GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            return null;
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        public string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }

        //string user = "user_chiselko6";D:\MAIN\Glinterion\Glinterion\App_Start\





        // DELETE: api/Photos/5
        [ResponseType(typeof(Photo))]
        public IHttpActionResult DeletePhoto(int id)
        {
            Photo photo = photosDb.GetPhotoById(id);
            if (photo == null)
            {
                return NotFound();
            }

            photosDb.DeletePhoto(id);
            photosDb.Save();

            return Ok(photo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                photosDb.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhotoExists(int id)
        {
            return photosDb.GetPhotos().Count(e => e.ID == id) > 0;
        }
    }

}