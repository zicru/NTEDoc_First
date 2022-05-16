using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public class RolePermissions
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public byte Active { get; set; }
    }
}
