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
    public class UnitOfWork<TContext> : IDisposable, IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        private readonly EntityDbContext _context;
        private IGenericRepository<Document> _documentRepository;
        private IGenericRepository<Sector> _sectorRepository;
        private IGenericRepository<DocumentType> _docTypeRepository;
        private IGenericRepository<Partner> _partnerRepository;
        private ILogRepository _logRepository;
        private IGenericRepository<Likvidatori> _likvidatorRepository;
        private IGenericRepository<Status> _statusRepository;

        public UnitOfWork(EntityDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Document> DocumentRepository
        {
            get
            {
                if (this._documentRepository == null)
                {
                    this._documentRepository = new GenericRepository<Document>(_context);
                }
                return _documentRepository;
            }
        }

        public IGenericRepository<Sector> SectorRepository
        {
            get
            {
                if (this._sectorRepository == null)
                {
                    this._sectorRepository = new GenericRepository<Sector>(_context);
                }
                return _sectorRepository;
            }
        }

        public IGenericRepository<DocumentType> DocTypeRepository
        {
            get
            {
                if (this._docTypeRepository == null)
                {
                    this._docTypeRepository = new GenericRepository<DocumentType>(_context);
                }
                return _docTypeRepository;
            }
        }

        public IGenericRepository<Partner> PartnerRepository
        {
            get
            {
                if (this._partnerRepository == null)
                {
                    this._partnerRepository = new GenericRepository<Partner>(_context);
                }
                return _partnerRepository;
            }
        }

        public ILogRepository LogRepository {
            get
            {
                if(this._logRepository == null)
                {
                    this._logRepository = new LogRepository(_context);
                }
                return _logRepository;
            }
        }

        public IGenericRepository<Likvidatori> LikvidatorRepository
        {
            get
            {
                if (this._likvidatorRepository == null)
                {
                    this._likvidatorRepository = new GenericRepository<Likvidatori>(_context);
                }
                return _likvidatorRepository;
            }
        }

        public IGenericRepository<Status> StatusRepository
        {
            get
            {
                if (this._statusRepository == null)
                {
                    this._statusRepository = new GenericRepository<Status>(_context);
                }
                return _statusRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
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


    }
}
