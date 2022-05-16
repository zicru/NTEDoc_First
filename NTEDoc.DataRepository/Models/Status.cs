using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public class Status : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Document> Document { get; set; }

        public ICollection<StatusChange> StatusChanges { get; set; }
    }
}
