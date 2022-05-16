using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    // The permissions values should match the values stored in the database
    public static class UserPermissions
    {
        public const int CanAddOrUpdateDocuments = 0;
        public const int CanReturnToSupplier = 1;
        public const int CanReturnToRecorder = 2;
        public const int CanSendToExecutor = 3;
        public const int CanConfirmReceiving = 4;
        public const int CanApproveDocuments = 5;
        public const int CanViewDocuments = 7;
        public const int CanAddOrUpdateUsers = 8;
        public const int CanViewUsers = 9;
        public const int CanSendToSectorInCharge = 10;
        public const int CanSignAndVerifyDocuments = 12;
        public const int CanSendToController = 13;
        public const int CanReturnToController = 14;
        public const int CanEditDocuments = 15;
    }
}
