using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    // The permissions values should match the values stored in the database
    public static class UserPermissions
    {
        // Document editing permissions
        public const int CanAddDocuments = 1;
        public const int CanEditDocuments = 2;
        public const int CanAddFilesToDocuments = 3;

        // Status changing permissions
        public const int CanSendToSectorInCharge = 10;
        public const int CanSignAndVerifyDocuments = 11;
        public const int CanReturnToController = 12;

        // Finalizing statuses
        public const int CanReturnToSupplier = 21;
        public const int CanApproveDocuments = 22;

        // Control and surveilance statuses
        public const int CanViewDocuments = 101;
        public const int CanViewUsers = 102;
        public const int CanAddOrUpdateUsers = 103;
    }
}
