using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static Dictionary<int, string> GetRuntimeErrorString = typeof(RuntimeErrors).GetFields()
            .Where(x => x.FieldType == typeof(int)).ToDictionary(x => (int) x.GetValue(null), x => x.Name);
    }
}
