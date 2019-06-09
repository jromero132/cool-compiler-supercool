using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCOOL.SemanticCheck;

namespace SuperCOOL.Core
{
    public class SymbolInfoNameComparer : IEqualityComparer<SymbollInfo>
    {
        public bool Equals(SymbollInfo x, SymbollInfo y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(SymbollInfo obj)
        {
            return obj.Name.GetHashCode();
        }
    }
    public class SymbollInfo 
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
        bool IsDefObject(string name,out SymbollInfo Info);
        bool IsDefObjectOnThis(string name, out SymbollInfo Info);
        ISymbolTable EnterScope();
        ISymbolTable ExitScope();
        void InheritsFrom(ISymbolTable parent);
        IList<SymbollInfo> AllDefinedObjects();
        IList<SymbollInfo> AllDefinedAttributes();
        IList<SymbollInfo> GetLocals();
    }

    public class SymbolTable : ISymbolTable
    {
        SymbolTable Parent;
        Dictionary<string, SymbollInfo> Objects;
        List<SymbollInfo> Locals;

        public SymbolTable(SymbolTable parent):this()
        {
            this.Parent=parent;
        }
        public SymbolTable()
        {
            Locals = new List<SymbollInfo>();
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
            var locals= Objects.Values.Where(x=>x.Kind==ObjectKind.Local).ToList();
            if (locals.Count > 0)
                Parent.Locals.AddRange(locals);
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
            
            return Objects.Values.Concat(Parent.AllDefinedObjects()).Distinct(new SymbolInfoNameComparer()).ToList();
        }

        public IList<SymbollInfo> AllDefinedAttributes()
        {
            var myAttributes = Objects.Values.Where(x => x.Kind == ObjectKind.Atribute);
            if (Parent == null)
                return myAttributes.ToList();

            return Parent.AllDefinedAttributes().Concat(myAttributes).ToList();
        }

        public IList<SymbollInfo> GetLocals()
        {
            return Locals;
        }
    }
}
