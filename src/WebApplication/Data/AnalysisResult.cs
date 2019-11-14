using System;
using System.Collections.Generic;
using Analysis.Analysis.ReachingDefinitions;
using System.Linq;
using Analysis.AST;
using Analysis.Analysis.DetectionOfSigns;

namespace WebApplication.Data
{
    public class AnalysisResult
    {
        public string Label { get; set; }
        public List<AnalysisIdentifier> NodeEntry { get; set; }
        public List<AnalysisIdentifier> NodeExit { get; set; }

    }
    public class AnalysisIdentifier
    {
        public string Name { get; set; }
        public List<string> Label { get; set; } = new List<string>();
        public string ID { get; set; }
        public AnalysisIdentifier() {}
        public AnalysisIdentifier(RDDefinition result) {
            Name = result.IdentityName.ToString();
            Label.Add(result.Label.ToString());
            ID = result.IdentityID.ToString();
        }
        public AnalysisIdentifier(Identifier result) {
            Name = result.Name.ToString();
            ID = result.Id.ToString();
        }
        public AnalysisIdentifier(KeyValuePair<Identifier, HashSet<DSSign>> result) {

            Name = result.Key.Name.ToString();
            ID = result.Key.Id.ToString();
            Label.AddRange(result.Value.Select(x=>x.ToString()));
        }

        //TODO: Inject the format string for each analysis
        public override string ToString() {
            string l = string.Join(", ", Label.Select(y => y.ToString()));
            return $"<kbd>{Name}</kbd> <span class='oi oi-arrow-right' aria-hidden='true'></span> {{ <var>{l}</var> }}<br/>";
        }
    }
}
