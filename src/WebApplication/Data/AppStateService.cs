
using System;

namespace WebApplication.Data
{
    public class AppStateService
    {
        public AnalysisType SelectedAnalysis { get; private set; } = GetDefaultAnalysis();
        public WorklistType SelectedWorklist { get; private set; } = GetDefaultWorklist();
        public event Action OnChange;


        public void SetAnalysisType(AnalysisType analysisType)
        {
            SelectedAnalysis = analysisType;
            NotifyStateChanged();
        }

        public void SetWorklistType(WorklistType worklistType)
        {
            SelectedWorklist = worklistType;
            NotifyStateChanged();
        }

        public void NotifyStateChanged() => OnChange?.Invoke();

        public static AnalysisType GetDefaultAnalysis()
        {
            return AnalysisType.ReachingDefinitions;
        }

        public static WorklistType GetDefaultWorklist()
        {
            return WorklistType.ChaoticIteration;
        }

    }
}