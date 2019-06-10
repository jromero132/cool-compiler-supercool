﻿using System;
using System.Text;

namespace SuperCOOL.CodeGeneration.MIPS
{
    public class MipsProgram
    {
        public StringBuilder SectionCode { get; private set; }
        public StringBuilder SectionData { get; private set; }
        public StringBuilder SectionFunctions { get; private set; }

        public MipsProgram()
        {
            this.SectionCode = new StringBuilder();
            this.SectionData = new StringBuilder();
            this.SectionFunctions = new StringBuilder();
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
            return new StringBuilder().Append(MipsGenerationHelper.NewScript().DataSection()).Append(SectionData)
                                        .Append(MipsGenerationHelper.NewScript().TextSection()).Append(SectionCode).Append(SectionFunctions).ToString();
        }
    }
}
