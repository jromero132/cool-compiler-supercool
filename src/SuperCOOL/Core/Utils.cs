using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.Core
{
    public static class Utils
    {
        public static bool IsSelf(this string name)
        {
            return name == "self";//TODO : me parece !!!
        }

        public static bool IsSelfType(this string name)
        {
            return name == "SELF_TYPE";
        }
    }
}
