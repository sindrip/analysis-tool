using System.Linq;
using Analysis.AST;
using Analysis.AST.Statement;
using Analysis.CFG;

namespace Analysis.Analysis.ReachingDefinitions
{
    public class RDAnalysis : Analysis<RDDomain>
    {
        private RDLattice _dummyLattice;

        public RDAnalysis(Program program) : base(program, AnalysisDirection.Forward)
        {
            _dummyLattice = new RDLattice(program);
            
            InitializeAnalysis();
            RunAnalysis();
        }
        
        protected override ILattice<RDDomain> Iota() => new RDLattice(_program);
        protected override ILattice<RDDomain> Bottom() => _dummyLattice.Bottom();
        
        protected override ILattice<RDDomain> TransferFunctions(int label)
        {
            var block = GetBlock(label);
            var kill = Kill(block);
            var gen = Gen(block);
            var domain = _analysisFilled[label].GetDomain();
            var newDomain = domain.Except(kill).Union(gen).ToDomain();
            return new RDLattice(newDomain);
        }

        public RDDomain Kill(IStatement block) => block switch
        {
            IntDecl intDecl => GetLabels().Select(l => new RDDefinition(intDecl.Id, l)).Append(new RDDefinition(intDecl.Id)).ToDomain(),
            ArrayDecl arrayDecl => GetLabels().Select(l => new RDDefinition(arrayDecl.Id, l)).Append(new RDDefinition(arrayDecl.Id)).ToDomain(),
            RecordDecl recordDecl => GetLabels().SelectMany(l => recordDecl.Fields.Select(f => new RDDefinition(f.Id, l))).ToDomain(),
            AssignStmt assignStmt => GetLabels().Select(l => new RDDefinition(assignStmt.Left.Left.Id, l))
                .Append(new RDDefinition(assignStmt.Left.Left.Id)).ToDomain(),
            RecAssignStmt recAssignStmt => GetLabels().SelectMany(l => recAssignStmt.Left.Children.Select(c => new RDDefinition(c.Id, l))).ToDomain(),
            _ => new RDDomain(),
        };

        public RDDomain Gen(IStatement block) => block switch
        {
            IntDecl intDecl => new RDDefinition(intDecl.Id, intDecl.Label).Singleton().ToDomain(),
            ArrayDecl arrayDecl => new RDDefinition(arrayDecl.Id, arrayDecl.Label).Singleton().ToDomain(),
            RecordDecl recordDecl => recordDecl.Fields.Select(f => new RDDefinition(f.Id, recordDecl.Label)).ToDomain(),
            AssignStmt assignStmt => new RDDefinition(assignStmt.Left.Left.Id, assignStmt.Label).Singleton().ToDomain(),
            RecAssignStmt recAssignStmt => recAssignStmt.Left.Children
                .Select(c => new RDDefinition(c.Id, recAssignStmt.Label)).ToDomain(),
            _ => new RDDomain(),
        };
        
        
    }
}