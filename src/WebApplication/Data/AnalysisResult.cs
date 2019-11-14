using System;
using System.Collections.Generic;
using Analysis.Analysis;
using Analysis.Analysis.ReachingDefinitions;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

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

        public override string ToString() {
            string l = string.Join(", ", Label.Select(y=>y.ToString()));
            return $"<kbd>{Name}</kbd> <span class='oi oi-arrow-right' aria-hidden='true'></span> {{ <var>{l}</var> }}<br/>";
        }
    }
}
