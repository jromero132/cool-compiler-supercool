using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.NameGenerator
{
    class LabelILGeneratorAutoincrement : ILabelILGenerator
    {
        private int ifIndex { get; set; }
        private int whileIndex { get; set; }
        public LabelILGeneratorAutoincrement()
        {
            ifIndex = whileIndex = 0;
        }
        public string GenerateIf()
        {
            return $"if_{ifIndex++}";
        }

        public string GenerateWhile()
        {
            return $"while_{whileIndex++}";
        }
    }
}
