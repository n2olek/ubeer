using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Validation;

using UBeer.Models;


namespace UBeer.Services
{
    // the class provides CRUD services
    public abstract class BaseService<T, U> where T : class where U : DbContext //: IndexedListItem, new()
    {
        private DbContext repo;
        private DbSet<T> dbSet;

        public BaseService(DbContext context)
        {
            this.repo = context;
            this.dbSet = GetRepo().Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> objT = dbSet;
            return objT;
        }

        public T GetByID(int id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<T> FilterByParam(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            try
            {
                return dbSet.Where(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert(T obj)
        {
            dbSet.Add(obj);
        }

        public void Update(T obj)
        {
            dbSet.Attach(obj);
            GetRepo().Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T ObjT = dbSet.Find(id);
            if (ObjT != null)
            {
                Delete(ObjT);
            }
        }

        public virtual void Delete(T obj)
        {
            if (GetRepo().Entry(obj).State == EntityState.Detached)
            {
                dbSet.Attach(obj);
            }
            dbSet.Remove(obj);
        }

        public void Save()
        {
            try
            {
                GetRepo().SaveChanges();
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.Write("*** Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.Write("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public U GetRepo()
        {
            return (U)repo;
        }
    }
}
