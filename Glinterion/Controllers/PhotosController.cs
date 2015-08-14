using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Glinterion.DAL;
using Glinterion.Models;

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

        //[System.Web.Http.HttpPost]
        //public JsonResult UploadFile()
        //{
        //    string Message, fileName;
        //    Message = fileName = string.Empty;
        //    bool flag = false;
        //    Request.
        //    if (Request.Files != null)
        //    {
        //        var file = Request.Files[0];
        //        fileName = file.FileName;
        //        try
        //        {
        //            file.SaveAs(Path.Combine(Server.MapPath("~/Images"), fileName));
        //            Message = "File uploaded";
        //            flag = true;
        //        }
        //        catch (Exception)
        //        {
        //            Message = "File upload failed! Please try again";
        //        }

        //    }
        //    return new JsonResult { Data = new { Message = Message, Status = flag } };
        //}

        // POST: api/Photos
        [ResponseType(typeof(Photo))]
        public IHttpActionResult PostPhoto(Photo photo)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Photos.Add(photo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = photo.ID }, photo);
        }

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