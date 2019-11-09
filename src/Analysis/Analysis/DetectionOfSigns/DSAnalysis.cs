using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.BExpr;
using Analysis.AST.Statement;

namespace Analysis.Analysis.DetectionOfSigns
{
    public class DSAnalysis : Analysis<DSDomain>

    {
        public DSAnalysis(Program program) : base(program, AnalysisDirection.Forward)
        {
            InitalizeAnalysis();
            RunAnalysis();
        }

        protected override ILattice<DSDomain> TransferFunctions(int label)
        {
            var block = GetBlock(label);
            var domain = _analysisFilled[label].GetDomain();
            var newDomain = block switch
            {
                AssignStmt assignStmt => AssignTransfer(assignStmt, domain),
                // TODO: recassign
                //RecAssignStmt recAssignStmt => RecAssignTransfer(recAssignStmt, domain),
                IfStmt ifStmt => ConditionTransfer(ifStmt.Condition, domain),
                IfElseStmt ifElseStmt => ConditionTransfer(ifElseStmt.Condition, domain),
                WhileStmt whileStmt => ConditionTransfer(whileStmt.Condition, domain),
                WriteStmt writeStmt => WriteTransfer(writeStmt, domain),
                ReadStmt readStmt => ReadTransfer(readStmt, domain),
                _ => DSLattice.Bottom(_program).GetDomain(),
            };
            return new DSLattice(newDomain);
        }

        private DSDomain CopyDomain(DSDomain domain)
        {
            var newDomain = new DSDomain();
            foreach (var pair in domain)
            {
                newDomain.Add(pair.Key, pair.Value.ToHashSet());
            }

            return newDomain;
        }

        private DSDomain AssignTransfer(AssignStmt assignStmt, DSDomain domain)
        {
            var ident = assignStmt.Left switch
            {
                VarAccess varAccess => varAccess.Left,
                ArrayAccess arrayAccess => arrayAccess.Left,
                RecordAccess recordAccess => recordAccess.Right,
            };

            var newDomain = CopyDomain(domain);

            var newValue = assignStmt.Left switch
            {
                VarAccess varAccess => DSUtil.Arithmetic(assignStmt.Right, domain),
                RecordAccess recordAccess => DSUtil.Arithmetic(assignStmt.Right, domain),
                ArrayAccess arrayAccess => DSUtil.Arithmetic(arrayAccess.Right, domain)
                    .Union(DSUtil.Arithmetic(assignStmt.Right, domain)),
            };

            newDomain[ident] = newValue.ToHashSet();
            return newDomain;
        }

        private DSDomain ConditionTransfer(IBExpr bExpr, DSDomain domain) => CopyDomain(domain);

        private DSDomain WriteTransfer(WriteStmt writeStmt, DSDomain domain) => CopyDomain(domain);

        private DSDomain ReadTransfer(ReadStmt readStmt, DSDomain domain)
        {
            var ident = readStmt.Left switch
            {
                VarAccess varAccess => varAccess.Left,
                ArrayAccess arrayAccess => arrayAccess.Left,
                RecordAccess recordAccess => recordAccess.Right,
            };

            var newDomain = CopyDomain(domain);

            newDomain[ident] = new HashSet<DSSign>() {DSSign.Negative, DSSign.Positive, DSSign.Zero};
            return newDomain;
        }

        protected override ILattice<DSDomain> Iota() => DSLattice.Top(_program);

        protected override ILattice<DSDomain> Bottom() => DSLattice.Bottom(_program);
    }
}