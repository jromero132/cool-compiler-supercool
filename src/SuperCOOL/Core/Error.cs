using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCOOL.Core
{
    public class Error
    {
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public ErrorKind ErrorKind { get; set; }

        public Error( string message, ErrorKind kind )
        {
            this.Message = message;
            this.ErrorKind = kind;
        }

        public Error( string message, ErrorKind kind, int line, int column ) : this( message, kind )
        {
            Line = line;
            Column = column;
        }

        public string ToString( IEnumerable<(string file_name, int line_limit)> limits )
        {
            var lims = limits.ToList();
            int p;
            for( p = 1 ; lims[ p ].line_limit <= this.Line ; ++p ) ;
            var line = this.Line - lims[ p - 1 ].line_limit;
            return $"Error at '{ lims[ p ].file_name }' => (Line { line }, Column { this.Column }) - { this.ErrorKind }: { this.Message }";
        }

    }


    public enum ErrorKind
    {
        CompilerError,
        LexicographicError,
        SyntacticError,
        NameError,
        TypeError,
        AttributeError,
        MethodError,
        SemanticError
    }
}
