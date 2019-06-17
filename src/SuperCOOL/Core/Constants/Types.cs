using System;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck;

namespace SuperCOOL.Constants
{
    public static class Types
    {
        public const string IO="IO";
        public const string Bool = "Bool";
        public const string Int = "Int";
        public const string String = "String";
        public const string Void = "Void";
        public const string Object = "Object";
        public const string SelfType= "SELF_TYPE";

        public static bool IsSelfType(string typeName)
        {
            return typeName == "SELF_TYPE";
        }

        public static bool IsSelf(string name)
        {
            return name == "self";
        }

        internal static bool IsStringBoolOrInt(string typeName)
        {
            return typeName == Int || typeName == String || typeName == Bool;
        }
    }
}
