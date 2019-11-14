using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analysis.Analysis.AvailableExpressions;
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
        public List<AnalysisResult> RunAnalysis(string source, AnalysisType analysisType)
        {
            var ast = Parser.Util.StringToAst(source);
            List<AnalysisResult> res = new List<AnalysisResult>();

            switch(analysisType)
            {
                case AnalysisType.ReachingDefinitions:
                {
                    var analysis = new RDAnalysis(ast);
                    var filledLattice = analysis.GetFilledLattice();
                    var circlelattice = analysis.GetCircleLattice();
                    
                    int label = 0;

                    var zipped = circlelattice.Zip(filledLattice, (entry, exit) => new AnalysisResult {
                        Label = (label++).ToString(),
                        NodeEntry = entry.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x)).ToList()                        // NodeEntry = new AnalysisIdentifier(entry.GetDomain().ToHashSet().ToList()),
                        .GroupBy(a => new {a.ID, a.Name})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList()
                        }).ToList(),
                        NodeExit = exit.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x)).ToList()
                        .GroupBy(a => new {a.ID, a.Name})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList()
                        }).ToList(),
                    }).ToList();
                    
                    return zipped;
                }
                case AnalysisType.AvailableExpressions:
                {
                    break;
                }
                default:
                {
                    break;
                }
            }

            return res;
        }
    }
}
