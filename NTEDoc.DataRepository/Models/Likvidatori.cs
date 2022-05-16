using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public class Likvidatori : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return this.FirstName + " " + this.LastName; } }
        public string UserId { get; set; }

        //public ICollection<Document> Document { get; set; }
    }
}
