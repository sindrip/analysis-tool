using System;
using System.IO;
using Antlr4.Runtime;

namespace Parser
{
    public class ThrowExceptionErrorListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg, RecognitionException e)
        {
            throw new ArgumentException($"Syntax error @ {line}:{charPositionInLine}", msg, e); 
        }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine,
            string msg, RecognitionException e)
        {
            throw new ArgumentException($"Syntax error @ {line}:{charPositionInLine}", msg, e); 
        }
    }
}