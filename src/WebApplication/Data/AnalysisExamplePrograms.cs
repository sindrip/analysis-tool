using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Data
{
    public class AnalysisExamplePrograms
    {
        public static Dictionary<string, string> GetPrograms()
        {
            return new Dictionary<string, string> ()
            {
                { "factorial", @"{
    int x;
    int y;
    x := 10;
    y := 1;
    while (x > 0) { 
        y := x * y;
        x := x -1; 
    } 
}" },
                { "test", "{\n  int x;\n  int w;\n  x := 0;\n  w := 1;\n}" }
            };
        }
    }
}
