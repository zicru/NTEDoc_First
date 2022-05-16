using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    [Table("firme_ugovori")]
    public class CompanyContract
    {
        [Column("IDugovorfirma")]
        public int CompanyContractId { get; set; }

        [Column("IDUgovora")]
        public int ContractId { get; set; }

        [Column("Arhiv_br")]
        public string ArchiveNumber { get; set; }

        [Column("naziv")]
        public string Name { get; set; }

        [Column("IDFirme")]
        public int CompanyId { get; set; }

        [Column("Datum_ug")]
        public DateTime? ContractDate { get; set; }

        [Column("Opis")]
        public string Description { get; set; }

        [Column("IDlokfirme")]
        public byte? OwnerId { get; set; }

        [Column("Aktivan")]
        public byte? Active { get; set; }

        [Column("Rok_ZAVR")]
        public DateTime? EndingDeadline { get; set; }

        [Column("IDVr_ugovora")]
        public byte? ContractTypeId { get; set; }

        [Column("Idaneksa")]
        public byte AnnexId { get; set; }

        public ICollection<Document> Documents { get; set; }

        [Column("IDReferenta")]
        public int? ExecutorId { get; set; }

        [Column("IDJNKontrolora")]
        public int? ControllerId { get; set; }
    }
}
