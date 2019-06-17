using System.Collections.Generic;

namespace SuperCOOL.Constants
{
    public static class RuntimeErrors
    {
        public const int DispatchOnVoid = 1;
        public const int CaseVoidRuntimeError = 2;
        public const int CaseWithoutMatching = 3;
        public const int DivisionBy0 = 4;
        public const int SubStringOutOfRange = 5;
        public const int HeapOverflow = 6;
        public const int ObjectAbort = 7;

        public static Dictionary<int, string> GetRuntimeErrorString = new Dictionary<int, string>
        {
            [DispatchOnVoid] = "",
            [CaseVoidRuntimeError] = "Match on void in case statement.",
            [CaseWithoutMatching] = "No match in case statement for Class ",
            [DivisionBy0] = "",
            [SubStringOutOfRange] = "",
            [HeapOverflow] = "",
            [ObjectAbort] = "Abort called from class "
        };
    }
}
