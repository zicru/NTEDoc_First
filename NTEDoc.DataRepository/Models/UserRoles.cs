using System;
using System.Collections.Generic;
using System.Text;

namespace NTEDoc.DataRepository.Models
{
    public static class UserRoles
    {
        public const int JNController = 1;
        public const int Executor = 2;
        public const int SectorRecorder = 3;
        public const int HelperRecorder = 4;

        public const int HigherJNController = 21;
        public const int HigherExecutor = 22;

        public const int Admin = 99;
        public const string SysAdmin = "99";
    }
}
