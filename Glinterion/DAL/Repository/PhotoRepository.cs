using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using Glinterion.DAL.IRepository;
using Glinterion.Models;

namespace Glinterion.DAL.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private GlinterionContext db;
        private IUserRepository users;

        public PhotoRepository(GlinterionContext context, IUserRepository usersRepository)
        {
            db = context;
            users = usersRepository;
        }

        public IQueryable<Photo> GetPhotos()
        {
            return db.Photos;
        }

        public IQueryable<Photo> GetPhotos(Expression<Func<Photo, bool>> predicate)
        {
            return (predicate == null ? GetPhotos() : db.Photos.Where(predicate));
        }

        public Photo GetPhoto(Expression<Func<Photo, bool>> predicate)
        {
            return (predicate == null ? null : db.Photos.FirstOrDefault(predicate));
        }

        public IQueryable<Photo> GetPhotos(string userLogin)
        {
            var user = users.GetUser(u => u.Login == userLogin);
            if (user == null)
            {
                return null;
            }
            int userId = user.UserId;
            return db.Photos.Where(photo => photo.User.UserId == userId);
        }

        public Photo GetPhoto(int id)
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