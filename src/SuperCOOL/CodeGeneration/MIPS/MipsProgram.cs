using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.MIPS
{
    public class MipsProgram
    {
        public StringBuilder SectionCode { get; }
        public StringBuilder SectionData { get; }
        public StringBuilder SectionFunctions { get; }

        public MipsProgram()
        {
            SectionCode = new StringBuilder();
            SectionData = new StringBuilder();
            SectionFunctions = new StringBuilder();
        }

        public static MipsProgram operator +( MipsProgram left, MipsProgram right )
        {
            var result = new MipsProgram();
            result.SectionCode.Append( left.SectionCode ).Append( right.SectionCode );
            result.SectionData.Append( left.SectionData ).Append( right.SectionData );
            result.SectionFunctions.Append( left.SectionFunctions ).Append( right.SectionFunctions );
            return result;
        }

        public override string ToString()
        {
            return new StringBuilder().Append(".data").Append(SectionData).Append(".text").Append(SectionCode).Append(SectionFunctions).ToString();
        }
    }
}
