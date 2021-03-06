using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Analysis.Analysis.DetectionOfSigns;
using Analysis.Analysis.FaintVariables;
using Analysis.Analysis.LiveVariables;
using Analysis.Analysis.ReachingDefinitions;
using Analysis.Analysis.IntervalAnalysis;

using Analysis.CFG;
using Analysis.Analysis;

namespace WebApplication.Data
{
    public class AnalysisService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private List<IterationStep> _iterationSteps;

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
        public List<AnalysisResult> RunAnalysis(string source, AnalysisType analysisType, WorklistType worklistType)
        {
            var ast = Parser.Util.StringToAst(source);
            List<AnalysisResult> res = new List<AnalysisResult>();

            switch(analysisType)
            {
                case AnalysisType.ReachingDefinitions:
                {
                    var analysis = new RDAnalysis(ast, worklistType.ToString());
                    var filledLattice = analysis.GetFilledLattice();
                    var circlelattice = analysis.GetCircleLattice();
                    _iterationSteps = analysis.GetIterationSteps();
                    
                    string htmlFormat = "<kbd>{0}</kbd> <span class='oi oi-arrow-right' aria-hidden='true'></span> {{ <var>{1}</var> }}<br/>";
                    
                    int label = 0;

                    var zipped = circlelattice.Zip(filledLattice, (entry, exit) => new AnalysisResult {
                        Label = (label++).ToString(),
                        NodeEntry = entry.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString,
                        }).ToList(),
                        NodeExit = exit.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString,
                        }).ToList(),
                    }).ToList();
                    
                    return zipped;
                }
                case AnalysisType.LiveVariables:
                {
                    var analysis = new LVAnalysis(ast, worklistType.ToString());
                    var filledLattice = analysis.GetFilledLattice();
                    var circlelattice = analysis.GetCircleLattice();
                    _iterationSteps = analysis.GetIterationSteps();
                    
                    int label = 0;
                    string htmlFormat = "<kbd>{0}</kbd> <span class='oi oi-arrow-right' aria-hidden='true'></span> Live <br/>";

                    var zipped = circlelattice.Zip(filledLattice, (entry, exit) => new AnalysisResult {
                        Label = (label++).ToString(),
                        NodeEntry = entry.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString
                        }).ToList(),
                        NodeExit = exit.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString
                        }).ToList(),
                    }).ToList();
                    
                    return zipped;
                }
                case AnalysisType.FaintVariables:
                {
                    var analysis = new FVAnalysis(ast, worklistType.ToString());
                    var filledLattice = analysis.GetFilledLattice();
                    var circlelattice = analysis.GetCircleLattice();
                    _iterationSteps = analysis.GetIterationSteps();

                    int label = 0;
                    string htmlFormat = "<kbd>{0}</kbd> <span class='oi oi-arrow-right' aria-hidden='true'></span> Strongly Live<br/>";

                    var zipped = circlelattice.Zip(filledLattice, (entry, exit) => new AnalysisResult {
                        Label = (label++).ToString(),
                        NodeEntry = entry.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString
                        }).ToList(),
                        NodeExit = exit.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString
                        }).ToList(),
                    }).ToList();
                    
                    return zipped;
                }
                case AnalysisType.DetectionOfSigns:
                {
                    var analysis = new DSAnalysis(ast, worklistType.ToString());
                    var filledLattice = analysis.GetFilledLattice();
                    var circlelattice = analysis.GetCircleLattice();
                    _iterationSteps = analysis.GetIterationSteps();
                    
                    int label = 0;
                    string htmlFormat = "<kbd>{0}</kbd> <span class='oi oi-arrow-right' aria-hidden='true'></span> {{ <var>{1}</var> }}<br/>";

                    var zipped = circlelattice.Zip(filledLattice, (entry, exit) => new AnalysisResult {
                        Label = (label++).ToString(),
                        NodeEntry = entry.GetDomain().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString
                        }).ToList(),
                        NodeExit = exit.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString
                        }).ToList(),
                    }).ToList();
                    
                    return zipped;
                }
                case AnalysisType.IntervalAnalysis:
                {
                    var analysis = new IAAnalysis(ast, worklistType.ToString());
                    var filledLattice = analysis.GetFilledLattice();
                    var circlelattice = analysis.GetCircleLattice();
                    _iterationSteps = analysis.GetIterationSteps();
                    
                    int label = 0;
                    string htmlFormat = "<kbd>{0}</kbd> <span class='oi oi-arrow-right' aria-hidden='true'></span> {{ <var>{1}</var> }}<br/>";

                    var zipped = circlelattice.Zip(filledLattice, (entry, exit) => new AnalysisResult {
                        Label = (label++).ToString(),
                        NodeEntry = entry.GetDomain().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString
                        }).ToList(),
                        NodeExit = exit.GetDomain().ToHashSet().Select(x=> new AnalysisIdentifier(x, htmlFormat)).ToList()
                        .GroupBy(a => new {a.ID, a.Name, a.FormatString})
                        .Select( b => new AnalysisIdentifier  {
                            Name = b.Key.Name,
                            ID = b.Key.ID,
                            Label = b.SelectMany(a=>a.Label).ToList(),
                            FormatString = b.Key.FormatString
                        }).ToList(),
                    }).ToList();
                    
                    return zipped;
                }
                default:
                {
                    break;
                }
            }

            return res;
        }


        public List<WorklistResult> GetIterationSteps()
        {
            List<WorklistResult> iterationSteps = new List<WorklistResult>();

            foreach(var entry in _iterationSteps)
            {
                WorklistResult currentStep = new WorklistResult(entry.CurrentStep, entry.CurrentEdge, entry.UpdatedWorklist, entry.AnalysisCircle);
                iterationSteps.Add(currentStep);
            }

            return iterationSteps;
        }
    }
}
