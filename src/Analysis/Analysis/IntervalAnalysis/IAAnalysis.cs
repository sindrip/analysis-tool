using System.Collections.Generic;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.Statement;

namespace Analysis.Analysis.IntervalAnalysis
{
    public class IAAnalysis : Analysis<IADomain>
    {

        public IAAnalysis(Program program) : base(program, AnalysisDirection.Forward)
        {
            InitializeAnalysis();
            RunAnalysis();
        }

        protected override ILattice<IADomain> TransferFunctions(int label)
        {
            var block = GetBlock(label);
            var domain = _analysisCircle[label].GetDomain();
            var newDomain = block switch
            {
                IntDecl intDecl => IntDeclTransfer(intDecl, domain),
                ArrayDecl arrayDecl => ArrayDeclTransfer(arrayDecl, domain),
                RecordDecl recordDecl => RecDeclTransfer(recordDecl, domain),
                AssignStmt assignStmt => AssignTransfer(assignStmt, domain),
                RecAssignStmt recAssignStmt => RecAssignTransfer(recAssignStmt, domain),
                IfStmt ifStmt => IdTransfer(ifStmt, domain),
                IfElseStmt ifElseStmt => IdTransfer(ifElseStmt, domain),
                WhileStmt whileStmt => IdTransfer(whileStmt, domain),
                WriteStmt writeStmt => IdTransfer(writeStmt, domain),
                ReadStmt readStmt => ReadTransfer(readStmt, domain),
                _ => Bottom().GetDomain(),
            };
            return new IALattice(newDomain);
        }

        private IADomain CopyDomain(IADomain domain)
        {
            var newDomain = new IADomain();
            foreach (var pair in domain)
            {
                newDomain.Add(pair.Key, pair.Value.Copy());
            }

            return newDomain;
        }

        private IADomain IntDeclTransfer(IntDecl intDecl, IADomain domain)
        {
            var newDomain = CopyDomain(domain);

            if (domain.IsBottom()) 
                return newDomain;
            
            var ident = new Identifier(intDecl.Name, VarType.Int, intDecl.Id);
            newDomain[ident] = new Interval(new ExtendedZ(0), new ExtendedZ(0));

            return newDomain;
        }

        private IADomain ArrayDeclTransfer(ArrayDecl arrayDecl, IADomain domain)
        {
            var newDomain = CopyDomain(domain);

            if (domain.IsBottom())
                return newDomain;
            
            var ident = new Identifier(arrayDecl.Name, VarType.Array, arrayDecl.Id);
            newDomain[ident] = new Interval(new ExtendedZ(0), new ExtendedZ(0));
            
            return newDomain;
        }

        private IADomain RecDeclTransfer(RecordDecl recordDecl, IADomain domain)
        {
            var newDomain = CopyDomain(domain);

            if (domain.IsBottom())
                return newDomain;

            foreach (var field in recordDecl.Fields)
            {
                newDomain[field] = new Interval(new ExtendedZ(0), new ExtendedZ(0));
            }

            return newDomain;
        }

        private IADomain IdTransfer(IStatement statement, IADomain domain) => CopyDomain(domain);

        private IADomain AssignTransfer(AssignStmt assignStmt, IADomain domain)
        {
            var newDomain = CopyDomain(domain);

            if (domain.IsBottom())
                return newDomain;

            var ident = assignStmt.Left switch
            {
                VarAccess varAccess => varAccess.Left,
                ArrayAccess arrayAccess => arrayAccess.Left,
                RecordAccess recordAccess => recordAccess.Right,
            };

            var newValue = assignStmt.Left switch
            {
                VarAccess varAccess => IAUtil.Arithmetic(assignStmt.Right, domain),
                RecordAccess recordAccess => IAUtil.Arithmetic(assignStmt.Right, domain),
                ArrayAccess arrayAccess => IAUtil.Arithmetic(arrayAccess.Right, domain)
                    .Join(IAUtil.Arithmetic(assignStmt.Right, domain)),
            };

            if (assignStmt.Left is ArrayAccess)
            {
                var ra = assignStmt.Left as ArrayAccess;
                var indexInterval = IAUtil.Arithmetic(ra.Right, domain);
                if (indexInterval.IsBottom)
                    return Bottom().GetDomain();
            }

            if (newValue.IsBottom)
                return Bottom().GetDomain();

            newDomain[ident] = newValue;
            return newDomain;
        }

        private IADomain RecAssignTransfer(RecAssignStmt recAssignStmt, IADomain domain)
        {
            var newDomain = CopyDomain(domain);

            if (domain.IsBottom())
                return newDomain;

            for (int i = 0; i < recAssignStmt.Left.Children.Count; i++)
            {
                var ident = recAssignStmt.Left.Children[i];
                var expr = recAssignStmt.Right[i];

                var newInterval = IAUtil.Arithmetic(expr, domain);
                if (newInterval.IsBottom)
                    return Bottom().GetDomain();

                newDomain[ident] = IAUtil.Arithmetic(expr, domain);
            }

            return newDomain;
        }

        private IADomain ReadTransfer(ReadStmt readStmt, IADomain domain)
        {
            var newDomain = CopyDomain(domain);

            if (domain.IsBottom())
                return newDomain;
            
            var ident = readStmt.Left switch
            {
                VarAccess varAccess => varAccess.Left,
                ArrayAccess arrayAccess => arrayAccess.Left,
                RecordAccess recordAccess => recordAccess.Right,
            };
            
            if (readStmt.Left is ArrayAccess)
            {
                var ra = readStmt.Left as ArrayAccess;
                var indexInterval = IAUtil.Arithmetic(ra.Right, domain);
                if (indexInterval.IsBottom)
                    return Bottom().GetDomain();
            }
            
            newDomain[ident] = new Interval(ExtendedZ.NegativeInfinity(), ExtendedZ.PositiveInfinity());
            return newDomain;
        }

        protected override ILattice<IADomain> Iota() => IALattice.Top(_program);

        protected override ILattice<IADomain> Bottom() => IALattice.Bottom(_program);
        
    }
}