using System;
using System.Collections.Generic;
using Analysis.Analysis.ReachingDefinitions;
using System.Linq;
using Analysis.AST;
using Analysis.Analysis.DetectionOfSigns;
using Analysis.Analysis.IntervalAnalysis;
using Analysis.CFG;

namespace WebApplication.Data
{
    public class WorklistResult
    {
        public int CurrentLabel;
        public List<FlowEdge> UpdatedWorklist;
        public int CurrentStep { get; set; }
        public List<(int, string)> AnalysisCircle;

        public WorklistResult(int currentStep, int currentLabel, List<FlowEdge> updatedWorklist, List<(int, string)> analysisCircle)
        {
            CurrentStep = currentStep;
            CurrentLabel = currentLabel;
            UpdatedWorklist = updatedWorklist;
            AnalysisCircle = analysisCircle;
        }
    }
}
