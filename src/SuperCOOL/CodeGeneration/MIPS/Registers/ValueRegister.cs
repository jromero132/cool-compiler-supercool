using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.MIPS.Registers
{
    public class ValueRegister : Register
    {
        public ValueRegister( string syntax ) : base( syntax, "Value Register" ) { }
    }
}
