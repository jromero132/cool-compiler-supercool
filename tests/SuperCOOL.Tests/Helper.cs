using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SuperCOOL.Tests
{
    static class Helper
    {
        private static readonly string ENDL = Environment.CommandLine;

        public static void CreateFile( string file_name, params string[] lines )
        {
            Directory.CreateDirectory( Path.GetDirectoryName( file_name ) );
            using( StreamWriter writer = new StreamWriter( file_name ) )
            {
                writer.Write( string.Join( ENDL, lines ) );
            }
        }

        public static void RunSpim(string file_name, string input_file = null, string output_file = null)
        {
            RunFile($"spim -f \"\"{ file_name }\"\"",input_file,output_file);
        }

        public static void RunCoolCompiler(string file_name,string output_file)
        {
            RunFile($"CompilerStanford/mycool \"\"{file_name}\"\"", null, output_file);
        }

        public static void RunCoolCompilerFrontend(string[] file_name, string output_file)
        {
            var cmd = $"CompilerStanford/frontend ";
            foreach (var item in file_name)
                cmd+=$" \"\"{item}\"\"";
            RunFile(cmd, null, output_file);
        }

        private static void RunFile(string command, string input_file = null, string output_file = null )
        {
            var cmd = $"wsl {command} { ( input_file is null ? "" : $" < \"\"{input_file}\"\"" ) }{ ( output_file is null ? "" : $" > \"\"{output_file}\"\"" ) }";
            var x = $"/c { cmd }".Replace('\\','/');
            Process.Start( "cmd", x ).WaitForExit();
        }

        public static void DeleteFile( string file_name ) => File.Delete( file_name );

        public static bool CompareFiles( string file_name1, string file_name2 )
        {
            using( StreamReader read1 = new StreamReader( file_name1 ), read2 = new StreamReader( file_name2 ) )
            {
                while( true )
                {
                    if( read1.EndOfStream && read2.EndOfStream )
                        return true;

                    if( read1.EndOfStream || read2.EndOfStream )
                        return false;

                    if( read1.ReadLine() != read2.ReadLine() )
                        return false;
                }
            }
        }

        public static void Normalize( string file_name )
        {
            string tmp;
            using( StreamReader reader = new StreamReader( file_name ) )
            {
                for( int i = 0 ; i < 5 ; i++ )
                    reader.ReadLine();
                tmp = reader.ReadToEnd();
            }
            using( StreamWriter writer = new StreamWriter( file_name ) )
            {
                writer.Write( tmp );
            }
        }
    }
}
