using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.Core
{
    public class CoolType
    {
        public string Name { get; private set; }
        public CoolType Parent { get; set; }
        public List<CoolType> Childs { get; set; }

        public ISymbolTable SymbolTable { get; set; }
        public List<SymbolInfo> Atributes{ get; private set; }
        public int AllocateSize => 4 * Atributes.Count();

        public CoolType(string Name) {
            this.Name = Name;
            Childs = new List<CoolType>();
            SymbolTable = new SymbolTable();
        }

        public CoolType(string Name, ISymbolTable symbolTable) : this(Name)
        {
            SymbolTable = symbolTable;
        }

        public override bool Equals( object obj )
        {
            if (this is NullType || obj is NullType)
                return true;
            CoolType coolType = obj as CoolType;
            return coolType.Name == this.Name;
        }

        public override int GetHashCode() => this.Name.GetHashCode();
        public override string ToString()
        {
            return Name;
        }
        public bool IsIt( CoolType Tatara )
        {
            if (this is NullType || Tatara is NullType) return true;

            if (this is SelfType self && Tatara is SelfType tatara)
                return self.ContextType == tatara.ContextType;
            if (this is SelfType me)
                return me.ContextType.IsIt(Tatara);
            if (Tatara is SelfType ancestor)
                return false;

            var type = this;
            if( type == Tatara )
                return true;
            while( type.Parent != null )
            {
                type = type.Parent;
                if( type.Equals( Tatara ) )
                    return true;
            }
            return false;
        }

        public void SetAttributes()
        {
            this.Atributes = SymbolTable.AllDefinedObjects().Where(x=>x.Kind==ObjectKind.Atribute).Select((x,i)=> { x.Offset = 4 * i;return x; }).ToList();
        }

    }

    public class SelfType : CoolType
    {
        public CoolType ContextType { get; set; }
        public SelfType(CoolType contextType) : base("SELF_TYPE")
        {
            ContextType = contextType;
        }
    }

    public class NullType : CoolType
    {
        public NullType() : base("_Null")
        {
        }
    }
}