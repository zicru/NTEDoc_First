using System;
using System.Collections.Generic;

namespace NTEDoc.DataRepository.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public short RoleLevel { get; set; }
        public string RoleId { get; set; }
        public string OrganisationUnitId { get; set; }
        public short OwnerId { get; set; }
        public string Program { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Documents assigned to user
        public ICollection<Document> ExecutorDocuments { get; set; }

        // Documents created by user
        public ICollection<Document> DocumentsOwned { get; set; }
        public ICollection<StatusChange> Logs { get; set; }
    }
}
