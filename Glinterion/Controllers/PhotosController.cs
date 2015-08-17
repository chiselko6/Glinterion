using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Glinterion.DAL;
using Glinterion.Models;
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

        [System.Web.Http.HttpPost]
        public JsonResult PostPhoto(HttpPostedFileBase upload, Photo photo)
        {
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            //bool flag = false;
            //if (Request.Files != null)
            //{
            //    var file = Request.Files[0];
            //    actualFileName = file.FileName;
            //    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            //    int size = file.ContentLength;

            //    try
            //    {
            //        file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles"), fileName));

            //        UploadedFile f = new UploadedFile
            //        {
            //            FileName = actualFileName,
            //            FilePath = fileName,
            //            Description = description,
            //            FileSize = size
            //        };
            //        using (MyDatabaseEntities dc = new MyDatabaseEntities())
            //        {
            //            dc.UploadedFiles.Add(f);
            //            dc.SaveChanges();
            //            Message = "File uploaded successfully";
            //            flag = true;
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        Message = "File upload failed! Please try again";
            //    }

            //}
            return new JsonResult { Data = new { Message = Message } };

        }

        // POST: api/Photos
        [System.Web.Http.HttpPost]
        public IHttpActionResult PostPhoto(HttpPostedFileBase upload)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = db.Photos.Count() + 1;
            Photo photo;
            if (upload != null)
            {
                string srcOriginal = @"images/user_chiselko6/original/img" + id + Path.GetExtension(upload.FileName);
                string srcPreview = @"images/user_chiselko6/preview/img" + id + Path.GetExtension(upload.FileName);
                var rating = 5.0;
                var description = "temp";
                var size = 1.4;
                photo = new Photo
                {
                    Description = description,
                    ID = id,
                    Rating = rating,
                    Size = size,
                    SrcOriginal = srcOriginal,
                    SrcPreview = srcPreview
                };
                // file is uploaded
                upload.SaveAs(srcOriginal);
                upload.SaveAs(srcPreview);
            }
            else
            {
                return null;
            }


            db.Photos.Add(photo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = photo.ID }, photo);
        }

        #region PostPhoto

        [System.Web.Http.HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = GetMultipartProvider();
            var result = await Request.Content.ReadAsMultipartAsync(provider);

            // On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
            // so this is how you can get the original file name
            var originalFileName = GetDeserializedFileName(result.FileData.First());

            // uploadedFileInfo object will give you some additional stuff like file length,
            // creation time, directory name, a few filesystem methods etc..
            var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

            // Remove this line as well as GetFormData method if you're not
            // sending any form data with your upload request
            var fileUploadObj = GetFormData<UploadDataModel>(result);

            // Through the request response you can return an object to the Angular controller
            // You will be able to access this in the .success callback through its data attribute
            // If you want to send something to the .error callback, use the HttpStatusCode.BadRequest instead
            var returnData = "ReturnTest";
            return this.Request.CreateResponse(HttpStatusCode.OK, new { returnData });
        }

        // You could extract these two private methods to a separate utility class since
        // they do not really belong to a controller class but that is up to you
        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            // IMPORTANT: replace "(tilde)" with the real tilde character
            // (our editor doesn't allow it, so I just wrote "(tilde)" instead)
            var uploadFolder = "~/images/user_chiselko6/original"; // you could put this to web.config
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }

        // Extracts Request FormatData as a strongly typed model
        private object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData
                    .GetValues(0).FirstOrDefault() ?? String.Empty);
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


        public class UploadDataModel
        {
            public string testString1 { get; set; }
            public string testString2 { get; set; }
        }

#endregion

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