using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NTEDoc.Models.ViewModels
{
    public class DocumentLight
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public string DocumentFileName { get; set; }
        public string DocumentNumber { get; set; }

        public int? CreatedByUser { get; set; }
        public int? ExecutorId { get; set; }
        public string ReceivedDate { get; set; }
        public string CurrencyDate { get; set; }
        public string LastStatusChangeDate { get; set; }
        public string ContractNumber { get; set; }
        public string Sector { get; set; }
        public string Partner { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public string? Comment { get; set; }
        public byte? SectorApproved { get; set; }
    }
}
