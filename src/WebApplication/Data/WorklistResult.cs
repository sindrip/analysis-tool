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

        public WorklistResult(int currentStep, FlowEdge? currentEdge, List<FlowEdge> updatedWorklist, List<(int, string)> analysisCircle)
        {
            CurrentStep = currentStep;
            CurrentEdge = currentEdge;
            UpdatedWorklist = updatedWorklist;
            AnalysisCircle = analysisCircle;
        }
    }
}
