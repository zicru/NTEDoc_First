using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public class StatusChange : IEntity
    {
        public int Id { get; set; }

        [ForeignKey("StatusId")]
        public DocumentStatus Status { get; set; }
        public int StatusId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User CreatedBy { get; set; }
        public int CreatedByUserId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }

        public string Comment { get; set; }

        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        public int? DocumentId { get; set; }

    }
}
