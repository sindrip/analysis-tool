using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public class SymbolTable
    {
        private SymbolTable _parent;
        private IList<Symbol> _symbols = new List<Symbol>();
        private static int _id = 0;

        public SymbolTable() => _id = 0;
        public SymbolTable(SymbolTable parent) => _parent = parent;

        public int InsertSymbol(string name, string type)
        {
            if (_symbols.Any(s => s.Name == name))
            {
                throw new ArgumentException($"Symbol with name {name} already declared in scope");
            }
            
            var symbol = new Symbol(name, type, _id++);
            _symbols.Add(symbol);
            return symbol.Id;
        }

        public Symbol LookupSymbol(string name)
        {

            var s = _symbols.FirstOrDefault(x => x.Name == name);
            if (s != null)
            {
                return s;
            }
            
            if (_parent == null)
            {
                return null;
            }
            
            return _parent.LookupSymbol(name);
        }

        public SymbolTable AddScope() => new SymbolTable(this);
        public SymbolTable RemoveScope() => this._parent;

        public class Symbol
        {
            public int Id;
            public string Name;
            public string Type;

            public Symbol(string name, string type, int id)
            {
                Name = name;
                Type = type;
                Id = id;
            }
        }
    }

}