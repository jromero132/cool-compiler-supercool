using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.MIPS.Registers
{
    public class ArgumentRegister : Register
    {
        public ArgumentRegister( string syntax ) : base( syntax, "Argument Register" ) { }
    }
}
