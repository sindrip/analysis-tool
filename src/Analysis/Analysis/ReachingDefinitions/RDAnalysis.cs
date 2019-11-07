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
            
            InitalizeAnalysis();
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
            // TODO: RecordDecl and AssignRecord (Needs work in parser to get ID's)
            IntDecl intDecl => GetLabels().Select(l => new RDDefinition(intDecl.Id, l, intDecl.Name)).Append(new RDDefinition(intDecl.Id, intDecl.Name)).ToDomain(),
            ArrayDecl arrayDecl => GetLabels().Select(l => new RDDefinition(arrayDecl.Id, l, arrayDecl.Name)).Append(new RDDefinition(arrayDecl.Id, arrayDecl.Name)).ToDomain(),
            // RecordDecl
            AssignStmt assignStmt => GetLabels().Select(l => new RDDefinition(assignStmt.Left.Left.Id, l, assignStmt.Left.Left.Name))
                .Append(new RDDefinition(assignStmt.Left.Left.Id, assignStmt.Left.Left.Name)).ToDomain(),
            // RecAssignStmt 
            _ => new RDDomain(),
        };

        public RDDomain Gen(IStatement block) => block switch
        {
            IntDecl intDecl => new RDDefinition(intDecl.Id, intDecl.Label, intDecl.Name).Singleton().ToDomain(),
            ArrayDecl arrayDecl => new RDDefinition(arrayDecl.Id, arrayDecl.Label, arrayDecl.Name).Singleton().ToDomain(),
            // TODO: RecordDecl and AssignRecord (Needs work in parser to get ID's)
            //RecordDecl arrayDecl => (RDDomain)(new RDDefinition(arrayDecl.Id, arrayDecl.Label)).Singleton(),
            AssignStmt assignStmt => new RDDefinition(assignStmt.Left.Left.Id, assignStmt.Label, assignStmt.Left.Left.Name).Singleton().ToDomain(),
            _ => new RDDomain(),
        };
        
        
    }
}