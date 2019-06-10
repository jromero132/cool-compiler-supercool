using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.NameGenerator
{
    class LabelILGeneratorAutoincrement : ILabelILGenerator
    {
        private int ifIndex { get; set; }
        private int exceptionIndex { get; set; }
        private int caseIndex { get; set; }
        private int variableIndex { get; set; }
        private int stringData { get; set; }
        public LabelILGeneratorAutoincrement()
        {
            ifIndex = caseIndex = variableIndex = stringData = 0;
        }

        public string GenerateVariable()
        {
            return $"_var_{variableIndex++}";
        }

        public (string end, string @else) GenerateIf()
        {
            return ($"_end_if_{ifIndex}", $"_else_if_{ifIndex++}");
        }

        public (string varInit, string endOfCase) GenerateCase()
        {
            return (GenerateVariable(), $"_caseEnd_{caseIndex++}");
        }

        public string GenerateFunc(string className,string methodName)
        {
            return $"{className}_{methodName}";
        }

        public string GenerateInit(string classTypeName)
        {
            return $"{classTypeName}__init";
        }

        public string GenerateStringData()
        {
            return $"_string_{stringData++}";
        }

        public string GenerateEmptyStringData()
        {
            return $"_string_empty";
        }

        public string GenerateLabelTypeName(string name)
        {
            return $"__{name}";
        }

        public string GenerateLabelTypeInfo(string name)
        {
            return $"___type_info_{name}";
        }

        public string GenerateLabelVirtualTable(string name)
        {
            return $"____virtual_table_{name}";
        }

        public string GetBuffer()
        {
            return $"_____buffer";
        }

        public string GetException()
        {
            return $"____exception{exceptionIndex++}";
        }
    }
}
