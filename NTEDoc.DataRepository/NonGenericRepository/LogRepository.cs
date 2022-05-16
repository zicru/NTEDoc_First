using Microsoft.EntityFrameworkCore;
using NTEDoc.DataRepository.Data;
using NTEDoc.DataRepository.GenericRepository;
using NTEDoc.DataRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTEDoc.DataRepository.NonGenericRepository
{
    public class LogRepository : GenericRepository<StatusChange>, ILogRepository
    {
        private readonly EntityDbContext _context;

        public LogRepository(EntityDbContext context)
        {
            this._context = context;
        }
        public IEnumerable<StatusChange> GetDocumentLog(int id)
        {
            return _context.StatusChanges.Where(a => a.DocumentId == id).ToList();
        }

        public void InsertLog(StatusChange obj)
        {
            _context.StatusChanges.Add(obj);
        }
    }
}
