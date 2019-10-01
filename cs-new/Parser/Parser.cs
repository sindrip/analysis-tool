using Analysis.AST;
using Antlr4.Runtime;
using Parser.Generated;

namespace Parser
{
    public static class Parser
    {
        public static Program StringToAst(string source)
        {
            var lexer = new MicroCLexer(new AntlrInputStream(source));
            
            var parser = new MicroCParser(new CommonTokenStream(lexer));
            var result = new ParseToAstVisitor().Visit(parser.parse());

            return result as Program;
        }
       
    }
}