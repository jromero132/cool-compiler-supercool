using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using SuperCOOL.ANTLR;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;
using System.IO;

namespace SuperCOOL
{
    class Compiler
    {
        static void Main(string[] args)
        {
            //string program = "";
            FileStream program = new FileStream(Directory.GetCurrentDirectory()+"/../../../"+"/Examples/atoi.cl",FileMode.Open);
            //Lexer
            SuperCOOLLexer superCOOLLexer = new SuperCOOLLexer(new AntlrInputStream(program));
            //Parser
            SuperCOOLParser superCOOLParser = new SuperCOOLParser(new CommonTokenStream(superCOOLLexer));
            IParseTree parseTree = superCOOLParser.program();
            //Build AST
            ASTNode ast = parseTree.Accept(new SuperCoolASTGeneratorVisitor());
            //Semantic Check
            ast.Accept(new SuperCoolClassDefSemanticCheckVisitor());
            ast.Accept(new SuperCoolTypeCheckVisitor());
            //...
        }
    }
}
