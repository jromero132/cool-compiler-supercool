using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using SuperCOOL.ANTLR;
using SuperCOOL.CodeGeneration;
using SuperCOOL.Core;
using SuperCOOL.NameGenerator;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperCOOL
{
    public class Compiler
    {
        const string COOL_EXTENSION = "cl";
        public static void Main( string[] args )
        {
            Console.WriteLine( "SuperCool Compiler Platform" );
            Console.WriteLine( "Copyright (C) Jose Ariel Romero & Jorge Yero Salazar & Jose Diego Menendez del Cueto. All rights reserved." );

            var Errors = Compile( args ,out var Code);
            //PrintErrors
            foreach( var item in Errors )
                Console.WriteLine( item );

            if (Errors.Count == 0)
                File.WriteAllText("out.mips", Code);
        }

        public static List<Error> Compile( string[] args,out string Code)
        {
            Code = null;
            string program = ProcessInput( args, out var Errors );
            //Lexer TODO: Lexer Errors
            SuperCOOLLexer superCOOLLexer = new SuperCOOLLexer( new AntlrInputStream( program ) );
            //Parser TODO: Parser Errors
            SuperCOOLParser superCOOLParser = new SuperCOOLParser( new CommonTokenStream( superCOOLLexer ) );
            IParseTree parseTree = superCOOLParser.program();
            //Build AST
            CompilationUnit compilationUnit = new CompilationUnit();
            ASTNode ast = parseTree.Accept( new SuperCoolASTGeneratorVisitor() );
            //TypeDef Check
            ast.Accept( new SuperCoolTypeDefsVisitor( compilationUnit ) );
            //MethodDef Check
            ast.Accept( new SuperCoolMethodDefsVisitor( compilationUnit ) );
            //Type Check
            ast.Accept( new SuperCoolTypeCheckVisitor( compilationUnit ) );
            Errors.AddRange( ast.SemanticCheckResult.Errors );
            if (Errors.Count > 0)
                return Errors;
            //Code Generation 
            var labelgen = new LabelILGeneratorAutoincrement();
            var astIl = ast.Accept(new SuperCoolCILASTVisitor(labelgen, compilationUnit));
            var mips = astIl.Accept(new CodeGeneration.MIPS.CodeGenerator(labelgen, compilationUnit));
            Code = mips.ToString();

            return Errors;
        }

        private static string ProcessInput( string[] args, out List<Error> Errors )
        {
            StringBuilder program = new StringBuilder();
            StreamReader reader;
            Errors = new List<Error>();
            foreach( var item in args )
            {
                if( !File.Exists( item ) )
                {
                    Errors.Add( new Error( $"File { item } does not exists.", ErrorKind.CompilerError ) );
                    continue;
                }
                if( item.Split( '.' ).Last() != COOL_EXTENSION )
                {
                    Errors.Add( new Error( $"File { item } is not a Cool file.", ErrorKind.CompilerError ) );
                    continue;
                }
                reader = new StreamReader( item );
                program.Append( reader.ReadToEnd() );
                program.Append( Environment.NewLine );
            }
            return program.ToString();
        }
    }
}
