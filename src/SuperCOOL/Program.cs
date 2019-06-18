using SuperCOOL.ANTLR;
using SuperCOOL.CodeGeneration;
using SuperCOOL.Core;
using SuperCOOL.NameGenerator;
using SuperCOOL.SemanticCheck;
using SuperCOOL.SemanticCheck.AST;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Antlr4.Runtime.Atn;

namespace SuperCOOL
{
    public class Compiler
    {
        const string COOL_EXTENSION = "cl";

        public static void Main( string[] args )
        {
            //ConsoleExtended.WriteLineWithDelay( "SuperCool Compiler Platform" );
            //ConsoleExtended.WriteLineWithDelay( "Copyright (C) Jose Ariel Romero & Jorge Yero Salazar & Jose Diego Menendez del Cueto. All rights reserved." );
            //Console.WriteLine();

            args = new[] { "..\\..\\..\\..\\..\\tests\\SuperCOOL.Tests\\Examples\\Cool\\hello_world.cl" };

            var Errors = Compile( args, out var Code, out var limits );
            //PrintErrors
            foreach( var item in Errors )
                Console.WriteLine( item.ToString( limits ) );

            if( Errors.Count() == 0 )
                File.WriteAllText( "out.mips", Code );
        }

        public static IEnumerable<Error> Compile( string[] args, out string Code, out IEnumerable<(string, int)> limits )
        {
            Code = null;
            string program = ProcessInput( args, out var Errors, out limits );
            if( Errors.Count > 0 )
                return Errors;
            var lexingsyntactycErrors = new CustomErrorListener();

            //Lexer
            SuperCOOLLexer superCOOLLexer = new SuperCOOLLexer( new AntlrInputStream( program ) );
            superCOOLLexer.RemoveErrorListeners();
            var tokenStream = new CommonTokenStream( superCOOLLexer );

            //Parser
            SuperCOOLParser superCOOLParser = new SuperCOOLParser( tokenStream );
            superCOOLParser.RemoveErrorListeners();
            superCOOLParser.AddErrorListener( lexingsyntactycErrors );
            IParseTree parseTree = superCOOLParser.program();
            if( lexingsyntactycErrors.Errors.Count > 0 )
                return lexingsyntactycErrors.Errors;

            //Semantyc Check
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
            if( Errors.Count > 0 )
                return Errors;

            //Code Generation 
            var labelgen = new LabelILGeneratorAutoincrement();
            var astIl = ast.Accept( new SuperCoolCILASTVisitor( labelgen, compilationUnit ) );
            var mips = astIl.Accept( new CodeGeneration.MIPS.CodeGenerator( labelgen, compilationUnit ) );
            Code = mips.ToString();

            return new List<Error>();
        }

        private static string ProcessInput( string[] args, out List<Error> Errors, out IEnumerable<(string, int)> limits )
        {
            int lines = 0;
            var _limits = new List<(string, int)> { ("", lines) };

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
                var text = reader.ReadToEnd();
                program.Append( text );
                program.Append( Environment.NewLine );
                _limits.Add( (item, lines += text.Count( c => c == '\n' ) + 2) );
            }
            limits = _limits;
            return program.ToString();
        }
    }

    public static class ConsoleExtended
    {
        public static void WriteWithDelay( object o, int milliseconds = 100 )
        {
            foreach( char c in o.ToString() )
            {
                Console.Write( c );
                Thread.Sleep( milliseconds );
            }
        }

        public static void WriteLineWithDelay( object o, int milliseconds = 100 ) => WriteWithDelay( o + Environment.NewLine, milliseconds );
    }
}
