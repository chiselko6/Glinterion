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
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Glinterion.Common;
using Glinterion.DAL;
using Glinterion.DAL.IRepository;
using Glinterion.DAL.Repository;
using Glinterion.Models;
using Glinterion.Services.PhotoServices;
using Newtonsoft.Json;
using Route = System.Web.Http.RouteAttribute;
using Authorize = System.Web.Http.AuthorizeAttribute;
using HttpGet = System.Web.Http.HttpGetAttribute;
using HttpPost = System.Web.Http.HttpPostAttribute;

namespace Glinterion.Controllers
{
    [System.Web.Http.Authorize]
    public class PhotosController : ApiController
    {
        private IGenericRepository<User> usersDb;
        private IGenericRepository<Role> rolesDb;
        private IGenericRepository<Account> accountsDb;
        private IGenericRepository<Photo> photosDb;
        private IUnitOfWork uof;
        private PhotoSaveService photoSaveService;
        
        public PhotosController()
        {
            uof = DependencyResolver.Current.GetService<IUnitOfWork>();
            accountsDb = uof.Repository<Account>();
            rolesDb = uof.Repository<Role>();
            usersDb = uof.Repository<User>();
            photosDb = uof.Repository<Photo>();

            photoSaveService = DependencyResolver.Current.GetService<PhotoSaveService>();
        }
        
        // GET: api/Photos
        [Authorize]
        [RolesAuthorize("admin", "moderator")]
        [Route("~/api/photos")]
        public IEnumerable<Photo> GetAllPhotos()
        {
            return photosDb.GetAll();
        }

        //private IEnumerable<Photo> Clean(IEnumerable<Photo> photos)
        //{
        //    photos = photos.AsEnumerable().Select(photo =>
        //    {
        //        var dumpUserAccount = photo.User.Account;
        //        photo.User.Account = null;
        //        var dumpUserPassword = photo.User.Password;
        //        photo.User.Password = null;
        //        var dumpUserRole = photo.User.Role;
        //        photo.User.Role = null;
        //        return photo;
        //    });
        //    return photos;
        //}

        [Route("~/api/photos/userphotos")]
        [HttpGet]
        public IEnumerable<Photo> GetPhotos()
        {
            var userName = User.Identity.Name;
            //return Clean(photosDb.GetAll(photo => photo.User.Login == userName));
            return photosDb.GetAll(photo => photo.User.Login == userName);

        }

        [HttpGet]
        [Route("~/api/photos/gcd")]
        public double TotalSize()
        {
            var userName = User.Identity.Name;
            var photos = photosDb.GetAll(photo => photo.User.Login == userName);
            if (photos == null)
            {
                return 0;
            }
            photos = photos.OrderBy(photo => photo.PhotoId);
            return photos.Sum(photo => photo.Size);
        }

        [HttpGet]
        [Route("~/api/photos/totalnumber")]
        public double TotalNumber()
        {
            var userName = User.Identity.Name;
            var photos = photosDb.GetAll( photo => photo.User.Login == userName);
            return photos == null ? 0 : photos.OrderBy(photo => photo.PhotoId).Count();
        }

        // GET: api/Photos
        [Route("~/api/photos")]
        public IEnumerable<Photo> GetPhotos(int pageNumber, int photosPerPage)
        {
            var userName = User.Identity.Name;
            var photos = photosDb.GetAll(photo => photo.User.Login == userName);
            if (photos == null)
            {
                return null;
            }
            if (photos.Count() >= pageNumber*photosPerPage)
            {
                //return Clean(photos.Skip((pageNumber - 1)*photosPerPage).Take(photosPerPage));
                return photos.Skip((pageNumber - 1)*photosPerPage).Take(photosPerPage);
            }
            if (photos.Count() > (pageNumber - 1)*photosPerPage)
            {
                //return Clean(photos.Skip((pageNumber - 1)*photosPerPage).Take(photos.Count() - (pageNumber - 1)*photosPerPage));
                return photos.Skip((pageNumber - 1)*photosPerPage).Take(photos.Count() - (pageNumber - 1)*photosPerPage);
            }
            return null;
        }

             //GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        [Authorize]
        [RolesAuthorize("admin", "moderator")]
        public IHttpActionResult GetPhoto(int id)
        {
            Photo photo = photosDb.GetById(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var userName = User.Identity.Name;
            var user = usersDb.Get(u => u.Login == userName);
            try
            {
                IEnumerable<string> descriptions = new List<string>();
                var description = (Request.Headers.TryGetValues("description", out descriptions) ? descriptions.First() : null);
                var ratings = Request.Headers.GetValues("rating");
                var rating = (ratings == null ? 0 : (ratings.First() == "null" ? 0 : Double.Parse(Request.Headers.GetValues("rating").First())));

                var dataStream = await provider.Contents[0].ReadAsStreamAsync();
                var fileExtension = Request.Headers.GetValues("fileExtension").FirstOrDefault();
                var size = dataStream.Length;
                var response = await photoSaveService.Save(dataStream, user, description, rating, fileExtension, false);
                return response;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        
        // DELETE: api/photos
        //[ResponseType(typeof(Photo))]
        //public IHttpActionResult DeletePhoto(int id)
        //{
        //    Photo photo = photosDb.GetById(id);
            
        //    photosDb.Delete(photo);
        //    photosDb.Save();

        //    return Ok(photo);
        //}

        private bool PhotoExists(int id)
        {
            return photosDb.GetAll().Count(e => e.PhotoId == id) > 0;
        }
    }

}