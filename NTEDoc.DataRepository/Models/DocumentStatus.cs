using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public static class DocumentStatus
    {
        public const int JNControllerReceivedDocument = 1;
        public const int JNControllerSentToSector = 2;
        public const int SectorSignedAndApproved = 3;
        public const int ReturnedToSupplier = 10;
        public const int Approved = 11; 

        public static List<int> JNControllerStatuses = new List<int>{
            1
        };

        public static List<int> SectorStatuses = new List<int>{
            2
        };

        public static List<int> ExecutorStatuses = new List<int>{
            3
        };
    }
}
