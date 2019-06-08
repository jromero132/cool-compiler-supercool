﻿using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Constants;
using SuperCOOL.Core;

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
            return $"_var_{variableIndex++}";
        }

        public string GenerateIf()
        {
            return $"_if_{ifIndex++}";
        }

        public (string varInit, string endOfCase) GenerateCase()
        {
            return (GenerateVariable(), $"_caseEnd_{caseIndex++}");
        }

        public string GenerateFunc(CoolMethod method)
        {
            return $"{method.Type}_{method.Name}";
        }

        public string GenerateInit(string classTypeName)
        {
            return $"{classTypeName}__init";
        }
    }
}
