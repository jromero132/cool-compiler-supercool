using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using SuperCOOL.ANTLR;
using SuperCOOL.Core;
using System;
using System.IO;

namespace SuperCOOL
{
    class Compiler
    {
        static void Main(string[] args)
        {
            //string program = "";
            FileStream program = new FileStream(Directory.GetCurrentDirectory()+"/../../../"+"/Examples/atoi.cl",FileMode.Open);
            SuperCOOLLexer superCOOLLexer = new SuperCOOLLexer(new AntlrInputStream(program));
            SuperCOOLParser superCOOLParser = new SuperCOOLParser(new CommonTokenStream(superCOOLLexer));
            IParseTree parseTree = superCOOLParser.program();
            CompilationUnit compilationUnit = new CompilationUnit();
            parseTree.Accept(new SemanticCheck.SuperCoolSemanticCheckVisitor(compilationUnit));
        }
    }
}
