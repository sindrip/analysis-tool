using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.Analysis.ReachingDefinitions;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.Statement;

namespace Analysis.Analysis.LiveVariables
{
    public class LVAnalysis : Analysis<LVDomain>
    {
        private LVLattice _dummyLattice;
        
        public LVAnalysis(Program program) : base(program, AnalysisDirection.Backward)
        {
            //_dummyLattice = new LVLattice(program);
            _dummyLattice = new LVLattice();
            
            InitializeAnalysis();
            RunAnalysis();
        }

        protected override ILattice<LVDomain> TransferFunctions(int label)
        {
            var block = GetBlock(label);
            var kill = Kill(block);
            var gen = Gen(block);
            var domain = _analysisFilled[label].GetDomain();
            var newDomain = domain.Except(kill).Union(gen).ToDomain();
            return new LVLattice(newDomain);
        }

        protected override ILattice<LVDomain> Iota() => new LVLattice();

        protected override ILattice<LVDomain> Bottom() => new LVLattice();

        public LVDomain Kill(IStatement block) => block switch
        {
            IntDecl intDecl => new Identifier(intDecl.Name, VarType.Int, intDecl.Id).Singleton().ToDomain(),
            RecordDecl recordDecl => recordDecl.Fields.ToDomain(),
            AssignStmt assignStmt => assignStmt.Left.Left.Singleton().ToDomain(),
            // TODO: Need full identifier here ..
            RecAssignStmt recAssignStmt => recAssignStmt.Left.Children.ToDomain(),
            ReadStmt readStmt => readStmt.Left.Left.Singleton().ToDomain(),
            _ => new LVDomain(),
        };

        private IEnumerable<Identifier> KillStateAccessHelper(IStateAccess stateAccess) => stateAccess switch
        {
            VarAccess varAccess => varAccess.Left.Singleton(),
            RecordAccess recordAccess => recordAccess.Right.Singleton(),
        _ => new HashSet<Identifier>(),
        };

        public LVDomain Gen(IStatement block) => block switch
        {
            AssignStmt assignStmt => AnalysisUtil.FreeVariables(assignStmt.Right).Union(GenStateAccessHelper(assignStmt.Left)).ToDomain(),
            RecAssignStmt recAssignStmt => recAssignStmt.Right.SelectMany(f => AnalysisUtil.FreeVariables(f)).ToDomain(),
            IfStmt ifStmt => AnalysisUtil.FreeVariables(ifStmt.Condition).ToDomain(),
            IfElseStmt ifElseStmt => AnalysisUtil.FreeVariables(ifElseStmt.Condition).ToDomain(),
            WhileStmt whileStmt => AnalysisUtil.FreeVariables(whileStmt.Condition).ToDomain(),
            WriteStmt writeStmt => AnalysisUtil.FreeVariables(writeStmt.Left).ToDomain(),
            ReadStmt readStmt => GenStateAccessHelper(readStmt.Left).ToDomain(),
            _ => new LVDomain(),
        };

        private IEnumerable<Identifier> GenStateAccessHelper(IStateAccess stateAccess) => stateAccess switch
        {
            ArrayAccess arrayAccess => AnalysisUtil.FreeVariables(arrayAccess.Right),
            _ => new HashSet<Identifier>(),
    };

    }
}