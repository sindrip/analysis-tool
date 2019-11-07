using System;

namespace WebApplication.Data
{
    public class AppStateService
    {
        public event Action OnChange;
        
        private string _sourceCode;
        public string SourceCode
        {
            get => _sourceCode;
            set
            {
                _sourceCode = value;
                NotifySourceCodeChanged();
            }
        }
        
        public string ParseError;
        public string FlowGraph;
        public AnalysisSelection SelectedAnalysis;

        public AppStateService()
        {
            SourceCode = "{ \n  int x;\n  x := 0;\n}";
            SelectedAnalysis = AnalysisSelection.ReachingDefinitions;
        }

        private void NotifySourceCodeChanged() => OnChange?.Invoke();
    }
}