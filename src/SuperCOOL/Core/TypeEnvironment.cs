using System;
using System.Collections.Generic;

namespace SuperCOOL.Core
{
    public class TypeEnvironment
    {
        public TypeEnvironment(string Type,TypeEnvironment Parent):this(Type)
        {
            ParentEnvironment = Parent;
        }
        public TypeEnvironment(string Type)
        {
            ObjectEnvironment = new Dictionary<string, CoolType>();
            MethodEnvironment = new Dictionary<string, CoolMethod>();
            CoolType = new CoolType(Type);
        }
        public TypeEnvironment ParentEnvironment { get; set; }
        Dictionary<string, CoolType> ObjectEnvironment { get; set; }
        Dictionary<string,CoolMethod> MethodEnvironment { get; set; }
        public CoolType CoolType { get; set; }

        public bool AddObject(string id,CoolType coolType)
        {
            return ObjectEnvironment.TryAdd(id, coolType);
        }

        internal bool IsDefO(string name)
        {
            return ObjectEnvironment.ContainsKey(name);
        }

        internal CoolType GetTypeO(string name)
        {
            if (name == "SelfType")
                return new SelfType(CoolType);
            ObjectEnvironment.TryGetValue(name,out CoolType result);
            return result;
        }
        public bool AddMethod(string Method,CoolMethod method)
        {
            return MethodEnvironment.TryAdd(Method, method);
        }
        internal bool IsDefMethod(string method)
        {
            return MethodEnvironment.ContainsKey(method);
        }

        internal CoolMethod GetMethod(string name)
        {
            MethodEnvironment.TryGetValue(name, out CoolMethod result);
            return result;
        }
    }
}
