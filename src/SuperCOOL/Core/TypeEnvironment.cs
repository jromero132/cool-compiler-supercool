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

        internal bool IsDefO(string name)
        {
            return ObjectEnvironment.ContainsKey(name);
        }

        internal string GetTypeO(string name)
        {
            ObjectEnvironment.TryGetValue(name,out string result);
            return result;
        }
    }
}
