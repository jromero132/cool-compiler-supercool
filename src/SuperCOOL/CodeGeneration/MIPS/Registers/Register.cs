using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.MIPS.Registers
{
    public class Register
    {
        private readonly string syntax;

        public Register( string syntax, string name = "Register" )
        {
            this.syntax = syntax;
            this.Name = name;
        }

        public string Name { get; }

        public override string ToString() => this.syntax;
    }
}
