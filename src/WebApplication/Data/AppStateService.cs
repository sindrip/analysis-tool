
using System;

namespace WebApplication.Data
{
    public class AppStateService
    {
        public AnalysisType SelectedAnalysis { get; private set; } = AnalysisType.ReachingDefinitions;
        public event Action OnChange;


        public void SetAnalysisType(AnalysisType analysisType)
        {
            SelectedAnalysis = analysisType;
            NotifyStateChanged();
        }

        
        public void NotifyStateChanged() => OnChange?.Invoke();


    }
}