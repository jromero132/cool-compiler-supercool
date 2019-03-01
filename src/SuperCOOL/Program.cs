using Antlr4.Runtime;
using SuperCOOL.ANTLR;
using System;

namespace SuperCOOL
{
    class Compiler
    {
        static void Main(string[] args)
        {
            //string program = args[0];
            string program = "";
            SuperCOOLLexer superCOOLLexer = new SuperCOOLLexer(new AntlrInputStream(program));
            SuperCOOLParser superCOOLParser = new SuperCOOLParser(new CommonTokenStream(superCOOLLexer));
        }
    }
}
