using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public class DeliveryType
    {
        public int DeliveryTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<Document> Documents { get; set; }
    }
}
