using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.MIPS.Registers
{
    public class StackPointer : Register
    {
        public StackPointer( string syntax ) : base( syntax, "Stack Pointer" ) { }
    }
}
