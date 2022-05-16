using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NTEDoc.DataRepository
{
    public class DocumentType : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Vrsta")]
        public string Naziv { get; set; }


        public ICollection<Document> Document { get; set; }
    }
}
