using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public class DocumentStatus : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte? Active { get; set; }


        public ICollection<Document> Document { get; set; }

        public ICollection<StatusChange> StatusChanges { get; set; }
    }
}
