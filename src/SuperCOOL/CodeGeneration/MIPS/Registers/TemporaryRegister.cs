using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.MIPS.Registers
{
    public class TemporaryRegister : Register
    {
        public TemporaryRegister( string syntax ) : base( syntax, "Temporary Register" ) { }
    }
}
