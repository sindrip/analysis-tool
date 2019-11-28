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
        public FlowEdge? CurrentEdge;
        public List<FlowEdge> UpdatedWorklist;
        public List<(int, string)> AnalysisCircle;

        public IterationStep(int currentStep, FlowEdge? currentEdge, List<FlowEdge> updatedWorklist, List<(int, string)> analysisCircle)
        {
            CurrentStep = currentStep;
            CurrentEdge = currentEdge;
            UpdatedWorklist = updatedWorklist;
            AnalysisCircle = analysisCircle;
        }
    }
}