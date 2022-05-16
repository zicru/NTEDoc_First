using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public class DocumentFile
    {
        public int DocumentFileId { get; set; }
        
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
        public int DocumentId { get; set; }

        public string FileTitle { get; set; }
        public byte? IsPrimaryFile { get; set; }
        public string FileName { get; set; }
        public byte? IsActive { get; set; }
    }
}
