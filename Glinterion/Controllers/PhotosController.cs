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
        private IImageRepository imagesDb;
        private IRoleRepository rolesDb;
        private IAccountRepository accountsDb;

        public PhotosController(IPhotoRepository photoRepository, 
                                IUserRepository userRepository, 
                                IImageRepository imageRepository,
                                IRoleRepository roleRepository,
                                IAccountRepository accountRepository)
        {
            photosDb = photoRepository;
            usersDb = userRepository;
            imagesDb = imageRepository;
            rolesDb = roleRepository;
            accountsDb = accountRepository;
        }
        
        // GET: api/Photos
        public IQueryable<Photo> GetPhotos()
        {
            return photosDb.GetPhotos().OrderBy(photo => photo.PhotoId);
        }

        public IQueryable<Photo> UserPhotos()
        {
            string userLogin = "RomanFrom710";
            return photosDb.GetPhotos(userLogin).OrderBy(photo => photo.PhotoId);
        }

        [HttpGet]
        public double TotalSize()
        {
            string userLogin = "RomanFrom710";
            var photos = photosDb.GetPhotos(userLogin);
            if (photos == null)
            {
                return 0;
            }
            photos = photos.OrderBy(photo => photo.PhotoId);
            return (photos == null ? 0 : photos.AsEnumerable().Sum(photo => photo.Size));
        }

        [HttpGet]
        public double TotalNumber()
        {
            string userLogin = "RomanFrom710";
            var photos = photosDb.GetPhotos(userLogin);
            if (photos == null)
            {
                return 0;
            }
            return photos.OrderBy(photo => photo.PhotoId).Count();
        }

        // GET: api/Photos
        public IQueryable<Photo> GetPhotos(int pageNumber, int photosPerPage)
        {
            var photos = photosDb.GetPhotos("RomanFrom710");
            if (photos == null)
            {
                return null;
            }
            photos = photos.OrderBy(photo => photo.PhotoId);
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
            Photo photo = photosDb.GetPhoto(id);
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

            if (id != photo.PhotoId)
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
            // RomanFrom710
            var user = usersDb.GetUser(1);
            try
            {
                IEnumerable<string> descriptions = new List<string>();
                string description = (Request.Headers.TryGetValues("description", out descriptions) ? descriptions.First() : null);
                
                var ratings = Request.Headers.GetValues("rating");
                double rating = (ratings == null ? 0 : (ratings.First() == "null" ? 0 : Double.Parse(Request.Headers.GetValues("rating").First())));

                // provider.Contents[0] supposed to be description
                //string description = await provider.Contents[0].ReadAsStringAsync();
                // provider.Contents[1] supposed to be rating
                //double rating = Double.Parse(await provider.Contents[1].ReadAsStringAsync());

                // provider.Contents[2] supposed to be file
                var dataStream = await provider.Contents[0].ReadAsStreamAsync();
                var size = dataStream.Length;
                imagesDb.Save(dataStream, user, description, rating);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent("Successful upload", Encoding.UTF8, "text/plain");
                response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(@"text/html");
                response.Content.Headers.Add("size", size.ToString());
                return response;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        
        // DELETE: api/photos
        [ResponseType(typeof(Photo))]
        public IHttpActionResult DeletePhoto(int id)
        {
            Photo photo = photosDb.GetPhoto(id);
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
            return photosDb.GetPhotos().Count(e => e.PhotoId == id) > 0;
        }
    }

}