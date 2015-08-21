using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glinterion.DAL.IRepository;

namespace Glinterion.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private GlinterionContext db;
        private Dictionary<string, object> repositories = new Dictionary<string, object>();

        public UnitOfWork(GlinterionContext db)
        {
            this.db = db;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var typeName = (typeof(TEntity)).Name;
            if (repositories.ContainsKey(typeName))
            {
                return repositories[typeName] as IGenericRepository<TEntity>;
            }
            var repository = DependencyResolver.Current.GetService<IGenericRepository<TEntity>>();
            repositories.Add(typeName, repository);
            return repository;
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}