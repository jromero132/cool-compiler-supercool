using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.Core
{
    public class Error
    {
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public ErrorKind ErrorKind { get; set; }
        public Error(string message,ErrorKind kind)
        {
            Message = message;
            ErrorKind = kind;
        }
        public Error(string message, ErrorKind kind,int line,int column):this(message,kind)
        {
            Line = line;
            Column = column;
        }
        public override string ToString()
        {
            return $"({Line},{Column}) - {ErrorKind}: {Message}";
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
