using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analysis.Analysis.ReachingDefinitions;
using Analysis.CFG;

namespace WebApplication.Data
{
    public class AnalysisService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }

        public string ParseStuff(string source)
        {
            var ast = Parser.Util.StringToAst(source);
            var fg = new FlowGraph(ast);
            var blocks = string.Join("\n", fg.Blocks.Select(s => s.PrintBlock()));
            var flow = string.Join("\n", fg.Edges);
            return blocks + "\n" + flow;
        }

        public string GetFlow(string source)
        {
            var ast = Parser.Util.StringToAst(source);
            var fg = new FlowGraph(ast);
            return string.Join("\n", fg.Edges);
        }

        public string GetBlocks(string source)
        {
            var ast = Parser.Util.StringToAst(source);
            var fg = new FlowGraph(ast);
            return string.Join("\n", fg.Blocks.Select(s => s.PrintBlock()));
        }

        public string GetGraph(string source)
        {
            var ast = Parser.Util.StringToAst(source);
            var fg = new FlowGraph(ast);
            return fg.ToGraphvizFormat();
        }
        public List<AnalysisResult> RunAnalysis(string source)
        {
            var res = new List<AnalysisResult>();
            
            var ast = Parser.Util.StringToAst(source);
            var analysis = new RDAnalysis(ast);
            var lattice = analysis.GetResultLattice();
            int i = 0;
            res.AddRange(from item in lattice
                         select new AnalysisResult
                         {
                             Label = (i++).ToString(),
                             Result = item.ToString()
                         });

            return res;
        }
    }
}