using NTEDoc.DataRepository.GenericRepository;
using NTEDoc.DataRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.NonGenericRepository
{
    public interface ILogRepository : IGenericRepository<StatusChange>
    {
        IEnumerable<StatusChange> GetDocumentLog(int id);
        void InsertLog(StatusChange obg);
    }
}
