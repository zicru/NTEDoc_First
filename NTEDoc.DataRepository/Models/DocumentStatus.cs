using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public static class DocumentStatus
    {
        public const int InProcessing = 0;
        public const int RecorderSentToJNController = 1;
        public const int JNControllerSentToExecutor = 2;
        public const int ExecutorReceivedFromJNController = 3;
        public const int ExecutorSentToSector = 4;
        public const int SectorSignedAndApproved = 5;
        public const int ExecutorReturnedToJNController = 6;
        public const int ExecutorReturnedToRecorder = 7;
        public const int JNControllerReturnedToRecorder = 9;
        public const int ReturnedToSupplier = 10;
        public const int Approved = 11; 

        public static List<int> RecorderStatuses = new List<int>{
            0, 7, 9
        };

        public static List<int> JNControllerStatuses = new List<int>{
            1, 6
        };

        public static List<int> ExecutorStatuses = new List<int>{
            2, 3, 5
        };

        public static List<int> SectorStatuses = new List<int>{
            4
        };
    }
}
