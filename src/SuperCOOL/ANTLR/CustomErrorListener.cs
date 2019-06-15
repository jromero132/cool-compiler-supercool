using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SuperCOOL.Core;
using System.Collections.Generic;

namespace SuperCOOL
{
    public class CustomErrorListener : IAntlrErrorListener<IToken>
    {
        public List<Error> Errors = new List<Error>();
        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            if (offendingSymbol!=null && offendingSymbol.Type == 48)
            {
                Errors.Add(new Error($"Invalid token {offendingSymbol.Text}", ErrorKind.LexicographicError, line, charPositionInLine));
                return;            
            }
            Errors.Add(new Error(msg, ErrorKind.SyntacticError, line, charPositionInLine));
        }
    }
}