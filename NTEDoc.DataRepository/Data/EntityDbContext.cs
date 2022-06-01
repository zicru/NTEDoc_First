using Microsoft.EntityFrameworkCore;
using NTEDoc.DataRepository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NTEDoc.DataRepository.Data
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options)
            : base(options)
        {
        }

        public EntityDbContext()
        {

        }

        public DbSet<Document> Document { get; set; }
        public DbSet<Sector> Sector { get; set; }
        public DbSet<DocumentType> DocumentType { get; set; }
        public DbSet<StatusChange> StatusChanges { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DocumentStatus> Statuses { get; set; }
        public DbSet<StatusTransaction> StatusTransactions { get; set; }
        public DbSet<Partner> Partner { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<CompanyContract> Contracts { get; set; }
        public DbSet<DocumentFile> DocumentFiles { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Dokument", "data");

                entity.Property(e => e.SectorId).HasColumnName("SectorId");

                entity.Property(e => e.DocumentTypeId).HasColumnName("TypeId");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReferenceNumber)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(d => d.DocumentsOwned)
                    .HasForeignKey(k => k.CreatedByUserId)
                    .HasConstraintName("FK_dataDokument_CreatedByUserId");

                entity.HasOne(e => e.Sector)
                    .WithMany(d => d.Document)
                    .HasForeignKey(k => k.SectorId)
                    .HasConstraintName("FK_dataDokument_SectorId");

                entity.HasOne(e => e.DocumentType)
                    .WithMany(d => d.Document)
                    .HasForeignKey(f => f.DocumentTypeId)
                    .HasConstraintName("FK_dataDocument_TypeId");

                entity.HasOne(e => e.Partner)
                    .WithMany(d => d.Document)
                    .HasForeignKey(f => f.PartnerId)
                    .HasConstraintName("FK_dataDocument_PartnerId");

                entity.HasOne(e => e.Likvidator)
                    .WithMany(d => d.ExecutorDocuments)
                    .HasForeignKey(f => f.LikvidatorId)
                    .HasConstraintName("FK_Dokument_Executor");

                entity.HasOne(e => e.Status)
                    .WithMany(d => d.Document)
                    .HasForeignKey(f => f.StatusId)
                    .HasConstraintName("fk_dataDocument_StatusId");

                entity.HasOne(e => e.CompanyContract)
                    .WithMany(e => e.Documents)
                    .HasForeignKey(e => e.CompanyContractId)
                    .HasConstraintName("FK_Dokument_CompanyContract");

                entity.HasMany(e => e.DocumentFiles)
                    .WithOne(d => d.Document)
                    .HasForeignKey(d => d.DocumentId);

                entity.HasMany(e => e.StatusChanges)
                    .WithOne(d => d.Document)
                    .HasForeignKey(d => d.DocumentId);

                entity.HasOne(e => e.DeliveryType)
                    .WithMany(e => e.Documents)
                    .HasForeignKey(e => e.DeliveryTypeId)
                    .HasConstraintName("FK_Dokument_DeliveryType");
            });

            modelBuilder.Entity<Sector>(entity =>
            {
                entity.ToTable("Sektor", "def");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Naziv)
                   .HasMaxLength(256)
                   .IsUnicode(false);

            });

            modelBuilder.Entity<DocumentFile>(entity =>
            {
                entity.HasOne(e => e.Document)
                    .WithMany(d => d.DocumentFiles)
                    .HasForeignKey(e => e.DocumentId)
                    .HasConstraintName("FK_DocumentFiles_Documents");
            });


            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("Partner", "data");
            });

            modelBuilder.Entity<DocumentStatus>(entity =>
            {
                entity.ToTable("Statusi", "def");
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.HasMany(e => e.StatusChanges)
                    .WithOne(e => e.Status)
                    .HasForeignKey(e => e.StatusId);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.ToTable("DocumentType", "def");
            });

            modelBuilder.Entity<StatusTransaction>(entity =>
            {
                entity.HasKey(x => new { x.StatusId, x.NextStatusId });
            });

            modelBuilder.Entity<StatusChange>(entity =>
            {
                entity.ToTable("StatusChanges", "data");

                entity.HasOne(e => e.Document)
                    .WithMany(l => l.StatusChanges)
                    .HasForeignKey(f => f.DocumentId)
                    .HasConstraintName("FK_DocumentId");

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(e => e.Logs)
                    .HasForeignKey(e => e.CreatedByUserId)
                    .HasConstraintName("FK_Log_User");

                entity.HasOne(e => e.Status)
                    .WithMany(e => e.StatusChanges)
                    .HasForeignKey(e => e.StatusId)
                    .HasConstraintName("FK_Log_StatusId");
            });

            modelBuilder.Entity<RolePermissions>(entity =>
            {
                entity.ToTable("RolePermissions");

                entity.HasKey(e => new { e.RoleId, e.PermissionId });
                
                entity.Property(e => e.RoleId).HasColumnType("int");
                entity.Property(e => e.PermissionId).HasColumnType("int");
            });

            modelBuilder.Entity<DeliveryType>(entity =>
            {
                entity.HasMany(e => e.Documents)
                    .WithOne(d => d.DeliveryType)
                    .HasForeignKey(d => d.DeliveryTypeId);
            });
        }
    }
}
