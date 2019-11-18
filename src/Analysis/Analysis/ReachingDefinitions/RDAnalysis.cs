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
            var domain = _analysisCircle[label].GetDomain();
            var newDomain = domain.Except(kill).Union(gen).ToDomain();
            return new RDLattice(newDomain);
        }
        

        public RDDomain Kill(IStatement block) => block switch
        {
            IntDecl intDecl => GetLabels().Select(l => new RDDefinition(intDecl.Id, l, intDecl.Name)).Append(new RDDefinition(intDecl.Id, intDecl.Name)).ToDomain(),
            ArrayDecl arrayDecl => GetLabels().Select(l => new RDDefinition(arrayDecl.Id, l, arrayDecl.Name)).Append(new RDDefinition(arrayDecl.Id, arrayDecl.Name)).ToDomain(),
            RecordDecl recordDecl => GetLabels().SelectMany(l => recordDecl.Fields.Select(f => new RDDefinition(f.Id, l, recordDecl.Name))).ToDomain(),
            AssignStmt assignStmt => GetLabels().Select(l => new RDDefinition(assignStmt.Left.Left.Id, l, assignStmt.Left.Left.Name))
                .Append(new RDDefinition(assignStmt.Left.Left.Id, assignStmt.Left.Left.Name)).ToDomain(),
            RecAssignStmt recAssignStmt => GetLabels().SelectMany(l => recAssignStmt.Left.Children.Select(c => new RDDefinition(c.Id, l, recAssignStmt.Left.Name))).ToDomain(),
            _ => new RDDomain(),
        };

        public RDDomain Gen(IStatement block) => block switch
        {
            IntDecl intDecl => new RDDefinition(intDecl.Id, intDecl.Label, intDecl.Name).Singleton().ToDomain(),
            ArrayDecl arrayDecl => new RDDefinition(arrayDecl.Id, arrayDecl.Label, arrayDecl.Name).Singleton().ToDomain(),
            RecordDecl recordDecl => recordDecl.Fields.Select(f => new RDDefinition(f.Id, recordDecl.Label, recordDecl.Name)).ToDomain(),
            AssignStmt assignStmt => new RDDefinition(assignStmt.Left.Left.Id, assignStmt.Label, assignStmt.Left.Left.Name).Singleton().ToDomain(),
            //TODO:
            RecAssignStmt recAssignStmt => recAssignStmt.Left.Children
                .Select(c => new RDDefinition(c.Id, recAssignStmt.Label, recAssignStmt.Left.Name)).ToDomain(),
            _ => new RDDomain(),
        };
        
        
    }
}