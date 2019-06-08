using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCOOL.SemanticCheck;

namespace SuperCOOL.Core
{
    public class SymbollInfo : IEquatable<SymbollInfo>
    {
        public SymbollInfo(string name, string type, ObjectKind kind)
        {
            Name = name;
            Type = type;
            Kind = kind;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public ObjectKind Kind { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SymbollInfo sinfo && sinfo.Name == this.Name;

        }

        public bool Equals(SymbollInfo other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
    public enum ObjectKind
    {
        Atribute,
        Parameter,
        Local,
        Self,
        ContextType,
    }
    public interface ISymbolTable
    {
        void DefObject(string name, string type, ObjectKind kind);
        bool IsDefObject(string name,out SymbollInfo Info);
        bool IsDefObjectOnThis(string name, out SymbollInfo Info);
        ISymbolTable EnterScope();
        ISymbolTable ExitScope();
        void InheritsFrom(ISymbolTable parent);
        IList<SymbollInfo> AllDefinedObjects();
    }

    public class SymbolTable : ISymbolTable
    {
        SymbolTable Parent;
        Dictionary<string, SymbollInfo> Objects;

        public SymbolTable(SymbolTable parent):this()
        {
            this.Parent=parent;
        }
        public SymbolTable()
        {
            Objects = new Dictionary<string,SymbollInfo>();
        }

        public void DefObject(string name, string type,ObjectKind kind)
        {
            Objects.Add(name,new SymbollInfo(name,type,kind));
        }

        public ISymbolTable EnterScope()
        {
            return new SymbolTable(this);
        }

        public ISymbolTable ExitScope()
        {
            return Parent;
        }

        public bool IsDefObject(string name, out SymbollInfo Info)
        {
            if (IsDefObjectOnThis(name,out Info))
                return true;
            return Parent?.IsDefObject(name,out Info)??false;
        }

        public bool IsDefObjectOnThis(string name, out SymbollInfo Info)
        {
            return Objects.TryGetValue(name,out Info);
        }

        public void InheritsFrom(ISymbolTable parent)
        {
            Parent =(SymbolTable)parent;
        }

        public IList<SymbollInfo> AllDefinedObjects()
        {
            if(Parent==null)
                return Objects.Values.ToList();
            
            return Objects.Values.Concat(Parent.AllDefinedObjects()).Distinct().ToList();
        }

    }
}
