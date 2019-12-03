using System.Collections.Generic;
using Analysis.CFG;

namespace WebApplication.Data
{
    public class WorklistResult
    {
        public FlowEdge? CurrentEdge;
        public List<FlowEdge> UpdatedWorklist;
        public int CurrentStep { get; set; }
        public List<(int, string)> AnalysisCircle;

        public LinkedList<FlowEdge> V;
        public LinkedList<FlowEdge> P;

        public WorklistResult(int currentStep, FlowEdge? currentEdge, List<FlowEdge> updatedWorklist, List<(int, string)> analysisCircle, LinkedList<FlowEdge> v, LinkedList<FlowEdge> p)
        {
            CurrentStep = currentStep;
            CurrentEdge = currentEdge;
            UpdatedWorklist = updatedWorklist;
            AnalysisCircle = analysisCircle;
            V = v;
            P = p;
        }
    }
}
