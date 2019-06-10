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
        public string RenamedSymbol { get; set; }
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
        bool IsDefObjectOnThis(string name, out SymbolInfo Info);
        ISymbolTable EnterScope();
        ISymbolTable ExitScope();
        void InheritsFrom(ISymbolTable parent);
        IList<SymbolInfo> AllDefinedObjects();
        IList<SymbolInfo> AllDefinedAttributes();
        IList<SymbolInfo> GetLocals();
    }

    public class SymbolTable : ISymbolTable
    {
        SymbolTable Parent;
        Dictionary<string, SymbolInfo> Objects;
        List<SymbolInfo> Locals;

        public SymbolTable(SymbolTable parent):this()
        {
            this.Parent=parent;
        }
        public SymbolTable()
        {
            Locals = new List<SymbolInfo>();
            Objects = new Dictionary<string,SymbolInfo>();
        }

        public void DefObject(string name, string type,ObjectKind kind)
        {
            Objects.Add(name,new SymbolInfo(name,type,kind));
        }

        public ISymbolTable EnterScope()
        {
            return new SymbolTable(this);
        }

        public ISymbolTable ExitScope()
        {
            var locals= Objects.Values.Where(x=>x.Kind==ObjectKind.Local).ToList();
            if (locals.Count > 0)
                Parent.Locals.AddRange(locals);
            return Parent;
        }

        public bool IsDefObject(string name, out SymbolInfo Info)
        {
            if (IsDefObjectOnThis(name,out Info))
                return true;
            return Parent?.IsDefObject(name,out Info)??false;
        }

        public bool IsDefObjectOnThis(string name, out SymbolInfo Info)
        {
            return Objects.TryGetValue(name,out Info);
        }

        public void InheritsFrom(ISymbolTable parent)
        {
            Parent =(SymbolTable)parent;
        }

        public IList<SymbolInfo> AllDefinedObjects()
        {
            if(Parent==null)
                return Objects.Values.ToList();
            
            return Objects.Values.Concat(Parent.AllDefinedObjects()).Distinct(new SymbolInfoNameComparer()).ToList();
        }

        public IList<SymbolInfo> AllDefinedAttributes()
        {
            var myAttributes = Objects.Values.Where(x => x.Kind == ObjectKind.Atribute);
            if (Parent == null)
                return myAttributes.ToList();

            return Parent.AllDefinedAttributes().Concat(myAttributes).ToList();
        }

        public IList<SymbolInfo> GetLocals()
        {
            return Locals;
        }
    }
}
