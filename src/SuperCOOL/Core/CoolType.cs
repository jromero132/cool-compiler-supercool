using System.Collections.Generic;

namespace SuperCOOL.Core
{
    public class CoolType
    {
        public CoolType Parent { get; set; }
        public List<CoolType> Childs { get; set; }

        public string Name { get; private set; }

        public CoolType(string Name) {
            this.Name = Name;
            Childs = new List<CoolType>(); 
        }

        public override bool Equals( object obj )
        {
            CoolType coolType = obj as CoolType;
            return coolType.Name == this.Name;
        }

        public override int GetHashCode() => this.Name.GetHashCode();

        public bool IsIt( CoolType Tatara )
        {
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
    }
}