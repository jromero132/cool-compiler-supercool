using SuperCOOL;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SuperCOOL.Tests.CoolTests
{
    class TestCase
    {
        public string TestCasePath { get; private set; }
        public string TestCaseName { get; private set; }

        public string CoolFileName => $"{ this.TestCaseName }.{ Default.CoolExtension }";
        public string CoolFileNamePath => $"{ Path.Combine( this.TestCasePath, this.CoolFileName ) }";

        public string MipsFromGoFileName => $"{ this.TestCaseName }_fromGo.{ Default.MipsExtension }";
        public string MipsFromGoFileNamePath => $"{ Path.Combine( this.TestCasePath, this.MipsFromGoFileName ) }";

        public string MipsFromUsFileName => $"{ this.TestCaseName }_fromUs.{ Default.MipsExtension }";
        public string MipsFromUsFileNamePath => $"{ Path.Combine( this.TestCasePath, this.MipsFromUsFileName ) }";

        public List<string> InputFileName { get; private set; }
        //public string InputFileNamePath => $"{ Path.Combine( this.TestCasePath, this.InputFileName ) }";

        public string OutputFileFromGoName => $"{ this.TestCaseName }_fromGo.{ Default.OutputExtension }";
        public string OutputFileFromGoPath => $"{ Path.Combine( this.TestCasePath, this.OutputFileFromGoName ) }";

        public string OutputFileFromUsName => $"{ this.TestCaseName }_fromUs.{ Default.OutputExtension }";
        public string OutputFileFromUsPath => $"{ Path.Combine( this.TestCasePath, this.OutputFileFromUsName ) }";


        public bool HasInputFile { get; private set; }

        public TestCase( string cool_file_path, string file_name = "", List<string> inputs_file = null )
        {
            file_name = file_name == "" ? Path.GetFileName( cool_file_path ) : file_name;
            this.TestCaseName = file_name;
            this.TestCasePath = Path.Combine( Default.Path, this.TestCaseName );
            this.InputFileName = inputs_file != null ? new List<string>(inputs_file) : new List<string>();

            Directory.CreateDirectory( this.TestCasePath );

            var path = Path.Combine( this.TestCasePath, this.CoolFileName );
            if( File.Exists( path ) )
                File.Delete( path );

            File.Copy( cool_file_path, path );
        }

        public bool RunTest()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = $"{ Default.GoCompilerPath }";
            p.StartInfo.Arguments = $"{ Path.Combine( this.TestCasePath, this.CoolFileName ) }";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            using( StreamWriter writer = new StreamWriter( $"{ this.MipsFromGoFileNamePath }" ) )
                writer.Write( p.StandardOutput.ReadToEnd() );
            p.WaitForExit();

            var errors = Compiler.Compile( new[] { this.CoolFileNamePath }, out string code,out var limits);
            using( StreamWriter writer = new StreamWriter( $"{ this.MipsFromUsFileNamePath }" ) )
                writer.Write( code );

            bool result = true;
            string currentFileName;
            int indexFile = 0;
            do
            {
                currentFileName = indexFile < InputFileName.Count ? InputFileName[indexFile++] : null;
                if (InputFileName.Count > 0 && currentFileName == null)
                    return result;
                Helper.RunSpim(this.MipsFromGoFileNamePath, currentFileName, this.OutputFileFromGoPath,
                    Default.GoCompilerTrapHandlerPath);
                Helper.RunSpim(this.MipsFromUsFileNamePath, currentFileName, this.OutputFileFromUsPath);

                Helper.Normalize(this.OutputFileFromGoPath, 3, CoolFileNamePath);
                Helper.Normalize(this.OutputFileFromUsPath);

                result &= Helper.CompareFiles(this.OutputFileFromGoPath, this.OutputFileFromUsPath);
            } while (currentFileName != null);

            return result;
        }
    }

    internal static class Default
    {
        public const string Path = "CoolTests";

        public const string CoolExtension = "cl";
        public const string MipsExtension = "mips";
        public const string InputExtension = "in";
        public const string OutputExtension = "out";
        public const string BatExtension = "bat";

        public const string GoCompiler = "codegen.exe";
        public static string GoCompilerPath => $"{ System.IO.Path.Combine( "GoCompiler", GoCompiler ) }";
        public const string GoCopilerTrapHandler = "trap.handler";
        public static string GoCompilerTrapHandlerPath => $"{ System.IO.Path.Combine( "GoCompiler", GoCopilerTrapHandler ) }";
    }
}
