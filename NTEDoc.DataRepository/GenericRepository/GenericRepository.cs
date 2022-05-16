using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using NTEDoc.DataRepository.Data;
using System.Security.Cryptography.X509Certificates;

namespace NTEDoc.DataRepository.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly EntityDbContext _context;
        private DbSet<T> table = null; //posto ne znamo unapred koji tip ce biti to znaci da ne mozemo pristupiti DbSet-u kao propertiju _contexta

        public GenericRepository()
        {
            this._context = new EntityDbContext();
        }

        public GenericRepository(EntityDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public void Delete(object id)
        {
            table.Remove(table.Find(id));
        }

        public IEnumerable<T> GetAll()
        {
            return table.AsNoTracking().ToList();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }


        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            table.Update(obj);

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<T> GetAll(string[] includes)
        {
            //var query = table.AsQueryable();
            //foreach (var include in includes) {
            //    query = query.Include(include);
            //}
            // return query;

            return includes.Aggregate(table.AsQueryable(), (query, includes) => query.Include(includes));

        }

        public IQueryable<T> GetById(object id, string[] includes)
        {
            //return includes.Aggregate(table.Where(a => a.Id == id).AsQueryable(), (query, includes) => query.Include(includes));
            var query = table.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Where(a => a.Id == Convert.ToInt32(id)).Include(include);
            }
            return query;
        }

       
    }
}
