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
        public LinkedList<FlowEdge> V;
        public LinkedList<FlowEdge> P;

        public IterationStep(int currentStep, FlowEdge? currentEdge, List<FlowEdge> updatedWorklist, List<(int, string)> analysisCircle, LinkedList<FlowEdge> v, LinkedList<FlowEdge> p)
        {
            CurrentStep = currentStep;
            CurrentEdge = currentEdge;
            UpdatedWorklist = updatedWorklist;
            AnalysisCircle = analysisCircle;
            V = new LinkedList<FlowEdge>(v);
            P = new LinkedList<FlowEdge>(p);
        }
    }
}