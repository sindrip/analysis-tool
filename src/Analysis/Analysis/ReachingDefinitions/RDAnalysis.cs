using System.Collections.Generic;
using System.Linq;
using Analysis.Analysis.IntervalAnalysis;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.Statement;
using Analysis.CFG;

namespace Analysis.Analysis.ReachingDefinitions
{
    public class RDAnalysis : Analysis<RDDomain>
    {
        private RDLattice _dummyLattice;

        public RDAnalysis(Program program, string worklistName) : base(program, AnalysisDirection.Forward, worklistName)
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
            RecordDecl recordDecl => GetLabels().SelectMany(l => recordDecl.Fields.Select(f => new RDDefinition(f.Id, l, f.Name)))
                .Concat(recordDecl.Fields.Select(f => new RDDefinition(f.Id, f.Name)))
                .ToDomain(),
            AssignStmt assignStmt => AssignKill(assignStmt),
            RecAssignStmt recAssignStmt => GetLabels().SelectMany(l => recAssignStmt.Left.Children.Select(c => new RDDefinition(c.Id, l, c.Name))).ToDomain(),
            _ => new RDDomain(),
        };

        private RDDomain AssignKill(AssignStmt assignStmt)
        {
            var newValue = assignStmt.Left switch
            {
                VarAccess varAccess => GetLabels().Select(l => new RDDefinition(varAccess.Left.Id, l, varAccess.Left.Name))
                    .Append(new RDDefinition(varAccess.Left.Id, varAccess.Left.Name)),
                ArrayAccess arrayAccess => GetLabels().Select(l => new RDDefinition(arrayAccess.Left.Id, l, arrayAccess.Left.Name))
                    .Append(new RDDefinition(arrayAccess.Left.Id, arrayAccess.Left.Name)),
                RecordAccess recordAccess => GetLabels().Select(l => new RDDefinition(recordAccess.Right.Id, l, recordAccess.Right.Name))
                    .Append(new RDDefinition(recordAccess.Right.Id, recordAccess.Right.Name)),
            };

            return newValue.ToDomain();
        }

        public RDDomain Gen(IStatement block) => block switch
        {
            IntDecl intDecl => new RDDefinition(intDecl.Id, intDecl.Label, intDecl.Name).Singleton().ToDomain(),
            ArrayDecl arrayDecl => new RDDefinition(arrayDecl.Id, arrayDecl.Label, arrayDecl.Name).Singleton().ToDomain(),
            RecordDecl recordDecl => recordDecl.Fields.Select(f => new RDDefinition(f.Id, recordDecl.Label, f.Name)).ToDomain(),
            AssignStmt assignStmt => AssignGen(assignStmt),
            RecAssignStmt recAssignStmt => recAssignStmt.Left.Children
                .Select(c => new RDDefinition(c.Id, recAssignStmt.Label, c.Name)).ToDomain(),
            _ => new RDDomain(),
        };
        
        private RDDomain AssignGen(AssignStmt assignStmt)
        {
            var newValue = assignStmt.Left switch
            {
                VarAccess varAccess => new RDDefinition(varAccess.Left.Id, assignStmt.Label, varAccess.Left.Name),
                ArrayAccess arrayAccess => new RDDefinition(arrayAccess.Left.Id, assignStmt.Label, arrayAccess.Left.Name),
                RecordAccess recordAccess => new RDDefinition(recordAccess.Right.Id, assignStmt.Label, recordAccess.Right.Name),
            };

            return newValue.Singleton().ToDomain();
        }
        
        
    }
}