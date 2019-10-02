using System;
using Analysis.AST;
using Antlr4.Runtime;
using Parser.Generated;

namespace Parser
{
    public static class Util
    {
        public static Program StringToAst(string source)
        {
            var lexer = new MicroCLexer(new AntlrInputStream(source));
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new ThrowExceptionErrorListener());
            
            var parser = new MicroCParser(new CommonTokenStream(lexer));
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ThrowExceptionErrorListener());
            
            var result = new ParseToAstVisitor().Visit(parser.parse());

            return result as Program;
        }
       
    }
}