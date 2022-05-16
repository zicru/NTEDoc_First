using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTEDoc.DataRepository.GenericRepository
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(string[] includes);
        T GetById(object id); //ova metoda prima obj umesto int jer razlicite tabele mogu imati razlicite tipove PK
        IQueryable<T> GetById(object id, string[] includes);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}
