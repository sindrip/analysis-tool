
using System;

namespace WebApplication.Data
{
    public class AppStateService
    {
        public AnalysisType SelectedAnalysis { get; private set; } = GetDefaultAnalysis();
        public event Action OnChange;


        public void SetAnalysisType(AnalysisType analysisType)
        {
            SelectedAnalysis = analysisType;
            NotifyStateChanged();
        }

        public void NotifyStateChanged() => OnChange?.Invoke();

        public static AnalysisType GetDefaultAnalysis()
        {
            return AnalysisType.ReachingDefinitions;
        }

    }
}