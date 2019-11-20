using Analysis.AST;
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
            var ident = new Identifier(intDecl.Name, VarType.Int, intDecl.Id);

            var newDomain = CopyDomain(domain);
            
            newDomain[ident] = new Interval(new ExtendedZ(0), new ExtendedZ(0));

            return newDomain;
        }

        private IADomain ArrayDeclTransfer(ArrayDecl arrayDecl, IADomain domain)
        {
            var ident = new Identifier(arrayDecl.Name, VarType.Array, arrayDecl.Id);

            var newDomain = CopyDomain(domain);
            
            newDomain[ident] = new Interval(new ExtendedZ(0), new ExtendedZ(0));
            
            return newDomain;
        }

        private IADomain RecDeclTransfer(RecordDecl recordDecl, IADomain domain)
        {
            var newDomain = CopyDomain(domain);

            foreach (var field in recordDecl.Fields)
            {
                newDomain[field] = new Interval(new ExtendedZ(0), new ExtendedZ(0));
            }

            return newDomain;
        }

        protected override ILattice<IADomain> Iota() => IALattice.Top(_program);

        protected override ILattice<IADomain> Bottom() => IALattice.Bottom(_program);
        
    }
}