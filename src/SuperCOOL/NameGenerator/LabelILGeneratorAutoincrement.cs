using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.NameGenerator
{
    class LabelILGeneratorAutoincrement : ILabelILGenerator
    {
        private int ifIndex { get; set; }
        private int caseIndex { get; set; }
        private int variableIndex { get; set; }
        public LabelILGeneratorAutoincrement()
        {
            ifIndex = caseIndex = variableIndex = 0;
        }

        public string GenerateVariable()
        {
            return $"var_{variableIndex++}";
        }

        public string GenerateIf()
        {
            return $"if_{ifIndex++}";
        }

        public (string varInit, string endOfCase) GenerateCase()
        {
            return (GenerateVariable(), $"caseEnd_{caseIndex++}");
        }
    }
}
