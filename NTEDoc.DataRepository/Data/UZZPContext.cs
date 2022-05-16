using Microsoft.EntityFrameworkCore;
using NTEDoc.DataRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Data
{
    public class UZZPContext : DbContext
    {
        public UZZPContext(DbContextOptions<UZZPContext> options)
            : base(options)
        {
        }

        public DbQuery<PartneriUgovori> V_firme_ugovori { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Query<PartneriUgovori>().ToView(
             "V_firme_ugovori");
        }
    }
}
