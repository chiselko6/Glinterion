﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Glinterion.DAL.IRepository;

namespace Glinterion.DAL.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private GlinterionContext context;
        private DbSet<TEntity> db;

        public GenericRepository(GlinterionContext context)
        {
            this.context = context;
            db = this.context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return db;
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return (db == null ? null : db.Where(predicate));
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return (db == null ? null : db.FirstOrDefault(predicate));
        }

        public TEntity GetById(int id)
        {
            return (db == null ? null : db.Find(id));
        }

        public void Add(TEntity entity)
        {
            db.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            db.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}