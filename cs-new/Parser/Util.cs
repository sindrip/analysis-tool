using System;
using System.IO;
using Analysis.AST;
using Antlr4.Runtime;
using Parser.Generated;

namespace Parser
{
    public static class Util
    {
        public static Program StringToAst(string source)
        {
            try
            {
                var lexer = new MicroCLexer(new AntlrInputStream(source));
                lexer.RemoveErrorListeners();
                lexer.AddErrorListener(new ThrowExceptionErrorListener());
                
                var parser = new MicroCParser(new CommonTokenStream(lexer));
                
                var result = new ParseToAstVisitor().Visit(parser.parse());

                return result as Program;
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
            
        }
       
    }

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