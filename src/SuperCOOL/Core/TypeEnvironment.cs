using System;
using System.Collections.Generic;

namespace SuperCOOL.Core
{
    public class TypeEnvironment
    {
        public TypeEnvironment ParentEnvironment { get; set; }
        Dictionary<string, string> ObjectEnvironment { get; set; }
        Dictionary<string,List<string>> MethodEnvironment { get; set; }
        public string CoolType { get; set; }

        public bool AddObject(string id,string coolType)
        {
            return ObjectEnvironment.TryAdd(id, coolType);
        }
        public bool AddMethod(string Method,List<string> types)
        {
            return MethodEnvironment.TryAdd(Method, types);
        }

        internal string GetObjectType()
        {
            throw new NotImplementedException();//TODO: Make Get Object Type....
        }
    }
}
