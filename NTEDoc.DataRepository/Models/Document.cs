using NTEDoc.DataRepository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NTEDoc.DataRepository
{
    public class Document : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Naziv")]
        public string Name { get; set; }

        [ForeignKey("CreatedByUserId")]
        [Display(Name = "Uneo u sistem")]
        public User CreatedBy { get; set; }

        [Required(ErrorMessage = "Izaberite evidenticara")]
        public int CreatedByUserId { get; set; }

        [Display(Name = "Datum unosa u sistem")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Datum prijema")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime ReceivedDate { get; set; }

        [Display(Name = "Datum dokumenta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime CurrencyDate { get; set; }

        [Display(Name="Iznos")]
        [Required(ErrorMessage = "Unesite iznos")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        public ICollection<DocumentFile> DocumentFiles { get; set; }

        [Display(Name = "Broj dokumenta")]
        public string DocumentNumber { get; set; }

        [Display(Name = "Delovodni broj")]
        public string ReferenceNumber { get; set; }

        [ForeignKey("CompanyContractId")]
        [Display(Name = "Broj ugovora")]
        public CompanyContract CompanyContract { get; set; }

        [AllowNull]
        public int? CompanyContractId { get; set; }

        [Display(Name = "Sektor (organizaciona jedinica):")]
        [ForeignKey("SectorId")]
        public Sector Sector { get; set; }
        
        [Required(ErrorMessage = "Izaberite sektor")]
        public int SectorId { get; set; }

        public enum EStatus { U_OBRADI = 0, PROSLEDJENO = 1, ODOBRENO = 2, ODBIJENO = 3, VRACENO_NA_DORADU = 4 }

        [ForeignKey("DocumentTypeId")]
        [Display(Name = "Vrsta dokumenta")]
        public DocumentType DocumentType { get; set; }

        public int? DocumentTypeId { get; set; }

        [ForeignKey("PartnerId")]
        [Display(Name = "Partner")]
        public Partner Partner { get; set; }

        [Required(ErrorMessage = "Izaberite partnera")]
        public int PartnerId { get; set; }

        [ForeignKey("LikvidatorId")]
        [Display(Name = "Likvidator")]
        public User Likvidator { get; set; }

        [Required(ErrorMessage = "Izaberite likvidatora")]
        public int LikvidatorId { get; set; }

        public int? ControllerId { get; set; }

        [NotMapped]
        public string ControllerName { get; set; }

        [ForeignKey("StatusId")]
        [Display(Name = "Status")]
        public Status Status { get; set; }

        public int? StatusId { get; set; }

        [AllowNull]
        public byte? SectorApproved { get; set; }

        public ICollection<StatusChange> StatusChanges { get; set; }

        [ForeignKey("DeliveryTypeId")]
        [Display(Name = "Način dostave")]
        public DeliveryType DeliveryType { get; set; }

        public int DeliveryTypeId { get; set; }

        public DateTime? LastStatusChangeDate { get; set; }

        public string? Comment { get; set; }
    }
}
