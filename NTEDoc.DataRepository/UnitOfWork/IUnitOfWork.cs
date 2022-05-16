using Microsoft.EntityFrameworkCore;
using NTEDoc.DataRepository.Data;
using NTEDoc.DataRepository.GenericRepository;
using NTEDoc.DataRepository.Models;
using NTEDoc.DataRepository.NonGenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.UnitOfWork
{
    public interface IUnitOfWork<out TContext> where TContext : DbContext, new()
    {
        public IGenericRepository<Document> DocumentRepository { get; }
        public IGenericRepository<Sector> SectorRepository { get; }
        public IGenericRepository<DocumentType> DocTypeRepository { get; }
        public IGenericRepository<Partner> PartnerRepository { get; }
        public ILogRepository LogRepository { get; }
        public IGenericRepository<Likvidatori> LikvidatorRepository { get; }
        public IGenericRepository<Status> StatusRepository { get; }

        void Dispose();
        void Save();
    }
}
