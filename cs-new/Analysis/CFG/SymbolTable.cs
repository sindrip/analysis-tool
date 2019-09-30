using System;
using System.Collections;
using System.Collections.Generic;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.Statement;

namespace Analysis.CFG
{
    public class SymbolTable
    {
        private int _uniqueId;

        private Dictionary<string, ISymbol> _table;

        private Program _prog;

        public SymbolTable(Program prog)
        {
            _prog = prog;
            _table = new Dictionary<string, ISymbol>();
            this.Setup(prog);
        }
        
        private void Setup(IAstNode node)
        {
            switch (node)
            {
                case Program program:
                    Setup(program.TopLevelStmt);
                    break;
                case ScopedBlock scopedBlock:
                    foreach (var statement in scopedBlock.Statements)
                    {
                        Setup(statement);
                    }
                    break;
                case UnscopedBlock unscopedBlock:
                    foreach (var statement in unscopedBlock.Statements)
                    {
                        Setup(statement);    
                    } 
                    break;
                case IntDecl intDecl:
                {
                    var n = new IntSymbol();
                    AddSymbol(intDecl.Name, n);
                    break;
                }
                case ArrayDecl arrayDecl:
                {
                    var a = new ArraySymbol();
                    AddSymbol(arrayDecl.Name, a);
                    break;
                }
                case RecordDecl recordDecl:
                {
                    // TODO: add record stuff
                    break;
                }
                case AssignStmt assignStmt:
                {
                    Setup(assignStmt.Left);
                    Setup(assignStmt.Right);
                    break;
                }
                case VarAccess varAccess:
                    LookupSymbol(varAccess.Name);
                    // TODO Do something here
                    break;
                case ArrayAccess arrayAccess:
                    LookupSymbol(arrayAccess.Left);
                    // TODO Do something here
                    break;
                case RecordAccess recordAccess:
                    LookupSymbol(recordAccess.Left);
                    // TODO some stuff
                    break;
                case ABinOp aBinOp:
                    Setup(aBinOp.Left);
                    Setup(aBinOp.Right);
                    break;
                case IStatement iStatement:
                    break;
                default:
                    throw new ApplicationException("Meta Blocks and Declarations only allowed");
            }
        }

        public void AddSymbol(string name, ISymbol symbol)
        {
            if (_table.ContainsKey(name))
            {
                throw new ApplicationException("Variable" + name + " defined more than once");
            }

            symbol.Name = name;
            symbol.Id = _uniqueId++;
            _table[name] = symbol;
        }

        public ISymbol LookupSymbol(string name)
        {
            if (_table.ContainsKey(name))
            {
                return _table[name];
            }

            return null;
        }


        
    }

    public interface ISymbol
    {
        string Name { get; set; }
        int Id { get; set; }
    }

    public class IntSymbol : ISymbol
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class ArraySymbol : ISymbol
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Size { get; set; }
    }

    public class RecordSymbol : ISymbol
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Size { get; set; }
        public IEnumerable<IntSymbol> Fields;
    }
}