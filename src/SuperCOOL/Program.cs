using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using SuperCOOL.ANTLR;
using SuperCOOL.Core;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;
using System.IO;
using System.Linq;

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
            CompilationUnit compilationUnit = new CompilationUnit();
            var astgenerator = new SuperCoolASTGeneratorVisitor();
            ASTNode ast = parseTree.Accept(astgenerator);
            var fase1=compilationUnit.BuildTypeEnvironment(astgenerator);
            
            //Type Check
            ast.Accept(new SuperCoolTypeCheckVisitor(compilationUnit));

            foreach (var item in fase1.Errors.Concat(ast.SemanticCheckResult.Errors))
                System.Console.WriteLine(item);
        }
    }
}
