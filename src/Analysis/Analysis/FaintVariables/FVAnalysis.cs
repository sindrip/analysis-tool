using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.BExpr;
using Analysis.AST.Statement;

namespace Analysis.Analysis.FaintVariables
{
    public class FVAnalysis : Analysis<FVDomain>
    {
        private FVLattice _dummyLattice;
        
        public FVAnalysis(Program program, string worklistName) : base(program, AnalysisDirection.Backward, worklistName)
        {
            _dummyLattice = new FVLattice();
            
            InitializeAnalysis();
            RunAnalysis();
        }

        protected override ILattice<FVDomain> TransferFunctions(int label)
        {
            var block = GetBlock(label);
            var domain = _analysisCircle[label].GetDomain();
            var newDomain = block switch
            {
                AssignStmt assignStmt => AssignTransfer(assignStmt, domain),
                RecAssignStmt recAssignStmt => RecAssignTransfer(recAssignStmt, domain),
                IfStmt ifStmt => ConditionTransfer(ifStmt.Condition, domain),
                IfElseStmt ifElseStmt => ConditionTransfer(ifElseStmt.Condition, domain),
                WhileStmt whileStmt => ConditionTransfer(whileStmt.Condition, domain),
                WriteStmt writeStmt => WriteTransfer(writeStmt, domain),
                ReadStmt readStmt => ReadTransfer(readStmt, domain),
                _ => new FVDomain(),
            };
            return new FVLattice(newDomain);
        }

        private FVDomain AssignTransfer(AssignStmt assignStmt, FVDomain domain)
        {
            var ident = assignStmt.Left switch
            {
                VarAccess varAccess => varAccess.Left,
                ArrayAccess arrayAccess => arrayAccess.Left,
                RecordAccess recordAccess => recordAccess.Right,
            };
            if (!domain.Contains(ident))
            {
                return domain;
            }

            var fv = AnalysisUtil.FreeVariables(assignStmt.Right);
            var fv2 = assignStmt.Left switch
            {
                ArrayAccess arrayAccess => AnalysisUtil.FreeVariables(arrayAccess.Right),
                _ => new HashSet<Identifier>(),
            };
            var freeVariables = fv.Union(fv2);
            return domain.Except(ident.Singleton()).Union(freeVariables).ToFVDomain();
        }

        private FVDomain RecAssignTransfer(RecAssignStmt recAssignStmt, FVDomain domain)
        {
            // This needs to be the children of the record
            var ident = recAssignStmt.Left;
            if (!domain.Contains(ident))
            {
                return domain;
            }

            var fv = recAssignStmt.Right.SelectMany(e => AnalysisUtil.FreeVariables(e));
            return domain.Except(ident.Singleton()).Union(fv).ToFVDomain();
        }

        private FVDomain ConditionTransfer(IBExpr bExpr, FVDomain domain) =>
            domain.Union(AnalysisUtil.FreeVariables(bExpr)).ToFVDomain();

        private FVDomain WriteTransfer(WriteStmt writeStmt, FVDomain domain) =>
            domain.Union(AnalysisUtil.FreeVariables(writeStmt.Left)).ToFVDomain();

        private FVDomain ReadTransfer(ReadStmt readStmt, FVDomain domain)
        {
            var ident = readStmt.Left switch
            {
                VarAccess varAccess => varAccess.Left,
                ArrayAccess arrayAccess => arrayAccess.Left,
                RecordAccess recordAccess => recordAccess.Right,
            };
            if (!domain.Contains(ident))
            {
                return domain;
            }

            return readStmt.Left switch
            {
                ArrayAccess arrayAccess => domain.Union(AnalysisUtil.FreeVariables(arrayAccess.Right)).ToFVDomain(),
                _ => domain.Except(ident.Singleton()).ToFVDomain(),
            };
        }

        protected override ILattice<FVDomain> Iota() => new FVLattice();

        protected override ILattice<FVDomain> Bottom() => new FVLattice();
        
    }
}