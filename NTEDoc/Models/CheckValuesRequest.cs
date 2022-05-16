using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTEDocOriginal.Models
{
    public class CheckValuesRequest
    {
        public int Partner { get; set; }
        public int DocumentType { get; set; }
        public string DocumentNumber { get; set; }
    }
}
