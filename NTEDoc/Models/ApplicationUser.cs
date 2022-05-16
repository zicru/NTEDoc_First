using Microsoft.AspNetCore.Identity;
using NTEDoc.DataRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NTEDoc.Models
{
    // This model is about to become redundant. Model in use will be "User"
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? SektorId { get; set; }
        [ForeignKey("SektorId")]
        public Sector Sector { get; set; }
    }
}
