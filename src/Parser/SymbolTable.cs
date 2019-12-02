using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.AST;

namespace Parser
{
    public class SymbolTable
    {
        private IList<IList<Symbol>> _symbols;
        private int _currentScope;
        private int _id = 0;

        public SymbolTable()
        {
            _symbols = new List<IList<Symbol>>();
            _currentScope = -1;
            _id = 0;
        }

        public int InsertSymbol(string name, VarType type)
        {
            var current = _symbols[_currentScope];
            if (current.Any(s => s.Name == name))
            {
                throw new ArgumentException($"Symbol with name {name} already declared in scope");
            }

            var symbol = new Symbol(name, type, _id++);
            current.Add(symbol);
            return symbol.Id;
        }

        public int InsertSymbol(string name, VarType type, int size)
        {
            var current = _symbols[_currentScope];
            if (current.Any(s => s.Name == name))
            {
                throw new ArgumentException($"Symbol with name {name} already declared in scope");
            }
            
            var symbol = new Symbol(name, type, _id++, size);
            current.Add(symbol);
            return symbol.Id;
        }

        public int InsertSymbol(string name, IList<Symbol> children)
        {
            var current = _symbols[_currentScope];
            if (current.Any(s => s.Name == name))
            {
                throw new ArgumentException($"Symbol with name {name} already declared in scope");
            }

            var symbol = new Symbol(name, VarType.Record, _id++, children);
            current.Add(symbol);
            return symbol.Id;
        }

        public Symbol LookupSymbol(string name) => LookupSymbol(_currentScope, name);

        private Symbol LookupSymbol(int scope, string name)
        {
            if (scope == -1)
            {
                throw new ArgumentException($"Use before declaration: {name}");
            }

            var current = _symbols[scope];

            var s = current.FirstOrDefault(x => x.Name == name);
            if (s != null)
            {
                return s;
            }

            return LookupSymbol(scope - 1, name);
        }

        public void AddScope()
        {
            _currentScope++;
            _symbols.Add(new List<Symbol>());
        }

        public IList<Symbol> RemoveScope()
        {
            var scopeToPop = _symbols[_currentScope];
            _symbols.RemoveAt(_currentScope);
            _currentScope--;
            return scopeToPop;
        }

        public class Symbol
        {
            public int Id;
            public string Name;
            public VarType Type;
            public int Size;
            public IList<Symbol> Children;

            public Symbol(string name, VarType type, int id)
            {
                Name = name;
                Type = type;
                Id = id;
            }

            public Symbol(string name, VarType type, int id, int size)
            {
                Name = name;
                Type = type;
                Id = id;
                Size = size;
            }

            public Symbol(string name, VarType type, int id, IList<Symbol> children)
            {
                Name = name;
                Type = type;
                Id = id;
                Children = children;
                Size = Children.Count;
            }
        }
    }
}
