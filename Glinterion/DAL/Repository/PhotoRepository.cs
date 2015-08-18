using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Glinterion.Models;

namespace Glinterion.DAL.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private PhotosContext db;

        public PhotoRepository(PhotosContext context)
        {
            db = context;
        }

        public IQueryable<Photo> GetPhotos()
        {
            return db.Photos;
        }

        public Photo GetPhotoById(int id)
        {
            return db.Photos.Find(id);
        }

        public void AddPhoto(Photo photo)
        {
            db.Photos.Add(photo);
        }

        public void DeletePhoto(int photoId)
        {
            var photo = db.Photos.Find(photoId);
            db.Photos.Remove(photo);
        }

        public void UpdatePhoto(Photo photo)
        {
            db.Entry(photo).State = EntityState.Modified;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}