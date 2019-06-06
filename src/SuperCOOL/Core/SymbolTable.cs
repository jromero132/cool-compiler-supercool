using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.SemanticCheck;

namespace SuperCOOL.Core
{
    public interface ISymbolTable
    {
        void DefObject(string name, string type);
        bool IsDefObject(string name,out string type);
        bool IsDefObjectOnThis(string name, out string type);
        ISymbolTable EnterScope();
        ISymbolTable ExitScope();
    }

    public class SymbolTable : ISymbolTable
    {
        SymbolTable Parent;
        Dictionary<string, string> Objects;

        public SymbolTable(SymbolTable parent):this()
        {
            this.Parent=parent;
        }
        public SymbolTable()
        {
            Objects = new Dictionary<string, string>();
        }

        public void DefObject(string name, string type)
        {
            Objects.Add(name, type);
        }

        public ISymbolTable EnterScope()
        {
            return new SymbolTable(this);
        }

        public ISymbolTable ExitScope()
        {
            return Parent;
        }

        public bool IsDefObject(string name, out string type)
        {
            if (IsDefObjectOnThis(name,out type))
                return true;
            return Parent?.IsDefObject(name,out type)??false;
        }

        public bool IsDefObjectOnThis(string name, out string type)
        {
            return Objects.TryGetValue(name,out type);
        }

    }
}
