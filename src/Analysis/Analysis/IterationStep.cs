using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Analysis.CFG;

namespace Analysis.Analysis
{
    public class IterationStep
    {
        public int CurrentStep;
        public int CurrentLabel;
        public List<FlowEdge> UpdatedWorklist;
        public List<(int, string)> AnalysisCircle;
        // figure out how to add analysis circle
        public IterationStep(int currentStep, int currentLabel, List<FlowEdge> updatedWorklist, List<(int, string)> analysisCircle)
        {
            CurrentStep = currentStep;
            CurrentLabel = currentLabel;
            UpdatedWorklist = updatedWorklist;
            AnalysisCircle = analysisCircle;
        }
    }
}