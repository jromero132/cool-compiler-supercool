using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCOOL.SemanticCheck;

namespace SuperCOOL.Core
{
    public class SymbolInfoNameComparer : IEqualityComparer<SymbolInfo>
    {
        public bool Equals(SymbolInfo x, SymbolInfo y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(SymbolInfo obj)
        {
            return obj.Name.GetHashCode();
        }
    }
    public class SymbolInfo 
    {
        public SymbolInfo(string name, string type, ObjectKind kind)
        {
            Name = name;
            Type = type;
            Kind = kind;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public ObjectKind Kind { get; set; }
        public int Offset { get; set; }
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
        bool IsDefObject(string name,out SymbolInfo Info);
        SymbolInfo GetObject(string stringLength);
        bool IsDefObjectOnThis(string name, out SymbolInfo Info);
        ISymbolTable EnterScope();
        ISymbolTable ExitScope();
        void InheritsFrom(ISymbolTable parent);
        IEnumerable<SymbolInfo> GetInSubScopes();
        IEnumerable<SymbolInfo> AllDefinedObjects();
    }

    public class SymbolTable : ISymbolTable
    {
        SymbolTable Parent;
        Dictionary<string, SymbolInfo> Objects;
        List<SymbolInfo> Subscopes;

        public SymbolTable(ISymbolTable parent) : this()
        {
            this.Parent = (SymbolTable)parent;
        }
        public SymbolTable()
        {
            Subscopes = new List<SymbolInfo>();
            Objects = new Dictionary<string, SymbolInfo>();
        }

        public void DefObject(string name, string type, ObjectKind kind)
        {
            Objects.Add(name, new SymbolInfo(name, type, kind));
        }

        public ISymbolTable EnterScope()
        {
            return new SymbolTable(this);
        }

        public ISymbolTable ExitScope()
        {
            var locals = Subscopes.ToList();
            var objects = Objects.Values.ToList();
            if (locals.Count > 0)
                Parent.Subscopes.AddRange(locals);
            if (objects.Count > 0)
                Parent.Subscopes.AddRange(objects);
            return Parent;
        }

        public bool IsDefObject(string name, out SymbolInfo Info)
        {
            if (IsDefObjectOnThis(name, out Info))
                return true;
            return Parent?.IsDefObject(name, out Info) ?? false;
        }

        public bool IsDefObjectOnThis(string name, out SymbolInfo Info)
        {
            return Objects.TryGetValue(name, out Info);
        }

        public SymbolInfo GetObject(string name)
        {
            IsDefObject(name, out var result);
            return result;
        }

        public void InheritsFrom(ISymbolTable parent)
        {
            Parent =(SymbolTable)parent;
        }

        public IEnumerable<SymbolInfo> AllDefinedObjects()
        {
            var mine = Objects.Values;
            if (Parent==null)
                return mine;
            return Parent.AllDefinedObjects().Concat(Objects.Values);
        }

        public IEnumerable<SymbolInfo> GetInSubScopes()
        {
            return Subscopes;
        }
    }
}
