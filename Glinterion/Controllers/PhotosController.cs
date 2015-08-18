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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
using Glinterion.DAL;
using Glinterion.Models;
using Glinterion.PhotoHelpers;
using Newtonsoft.Json;

namespace Glinterion.Controllers
{
    public class PhotosController : ApiController
    {
        private PhotosContext db = new PhotosContext();

        // GET: api/Photos
        public IQueryable<Photo> GetPhotos()
        {
            return db.Photos;
        }

        // GET: api/Photos/
        public IQueryable<Photo> GetPhotos(int startId, int endId)
        {
            if (endId < startId)
                return null;
            var photos = db.Photos;
            if (photos.Count() < startId)
                return null;
            return photos.OrderBy(photo => photo.ID).Skip(startId - 1).Take(endId - startId + 1);
        }

        // GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        public IHttpActionResult GetPhoto(int id)
        {
            Photo photo = db.Photos.Find(id);
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

            db.Entry(photo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

        public class UploadDataModel
        {
            public string testString1 { get; set; }
            public string testString2 { get; set; }
        }

        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        public async Task<HttpResponseMessage> Upload(string description = "", double rating = 3.0)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                int id = db.Photos.Count() + 1;
                foreach (var file in provider.Contents)
                {
                    var dataStream = await file.ReadAsStreamAsync();
                    byte[] bufferOriginal = new byte[dataStream.Length];
                    await dataStream.ReadAsync(bufferOriginal, 0, (int)dataStream.Length);
                    // TODO:
                    var user = "gcd";
                    var uploadFolderOriginal = "~/images/user_" + user + "/original/";
                    var uploadFolderPreview = "~/images/user_" + user + "/preview/";
                    var rootOriginal = HttpContext.Current.Server.MapPath(uploadFolderOriginal);
                    var rootPreview = HttpContext.Current.Server.MapPath(uploadFolderPreview);
                    Directory.CreateDirectory(rootOriginal);
                    Directory.CreateDirectory(rootPreview);

                    // TODO: convert to a smaller size
                    byte[] bufferPreview = bufferOriginal;
                    
                    rootOriginal += "img" + id + ".jpg";
                    rootPreview += "img" + id + ".jpg";
                    using (var stream = new FileStream(rootOriginal, FileMode.OpenOrCreate))
                    {
                        await stream.WriteAsync(bufferOriginal, 0, (int)bufferOriginal.Length);
                    }
                    using (var stream = new FileStream(rootPreview, FileMode.OpenOrCreate))
                    {
                        await stream.WriteAsync(bufferPreview, 0, (int)bufferPreview.Length);
                    }

                    // use the data stream to persist the data to the server (file system etc)
                }
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
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return NotFound();
            }

            db.Photos.Remove(photo);
            db.SaveChanges();

            return Ok(photo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhotoExists(int id)
        {
            return db.Photos.Count(e => e.ID == id) > 0;
        }
    }

}