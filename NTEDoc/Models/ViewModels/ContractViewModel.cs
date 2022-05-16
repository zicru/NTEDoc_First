using System;
using System.Collections.Generic;

namespace NTEDoc.Models.ViewModels
{
    public class ContractViewModel
    {
        public int CompanyContractId { get; set; }

        public int ContractId { get; set; }

        public string ArchiveNumber { get; set; }

        public string Name { get; set; }

        public int CompanyId { get; set; }

        public string ContractDate { get; set; }

        public string Description { get; set; }

        public byte? OwnerId { get; set; }

        public byte? Active { get; set; }

        public string EndingDeadline { get; set; }

        public byte? ContractTypeId { get; set; }

        public byte AnnexId { get; set; }

        public double TotalSum { get; set; }

        public int? ExecutorId { get; set; }
    }
}