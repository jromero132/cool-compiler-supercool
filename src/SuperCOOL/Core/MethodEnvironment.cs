using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCOOL.Core
{
    public class MethodEnvironment : IMethodEnvironment
    {
        public MethodEnvironment()
        {
            Methods = new Dictionary<CoolType, List<CoolMethod>>();
        }
        Dictionary<CoolType, List<CoolMethod>> Methods { get; set; }
        public void AddMethod(CoolType type, string method, List<CoolType> formals, CoolType returnType)
        {
            if (!Methods.ContainsKey(type))
                Methods[type] = new List<CoolMethod>();
            Methods[type].Add(new CoolMethod(method, formals, returnType));
        }
        public bool GetMethod(CoolType type, string method, out CoolMethod CoolMethod)
        {
            CoolMethod = new NullMethod(method);
            if (type is NullType)
                return true;
            if (type is SelfType selftype)
                return GetMethod(selftype.ContextType,method,out CoolMethod);
            if (GetMethodOnIt(type, method, out CoolMethod))
                return true;
            if (type.Parent != null)

                return GetMethod(type.Parent, method, out CoolMethod);
            return false;
        }
        public bool GetMethodOnIt(CoolType type, string method, out CoolMethod CoolMethod)
        {
            if (!Methods.ContainsKey(type))
                Methods[type] = new List<CoolMethod>();
            CoolMethod = Methods[type].FirstOrDefault(x => x.Name == method);
            return CoolMethod != null;
        }

        public IList<CoolMethod> GetVirtualTable(CoolType type)
        {
            var currentVirtualTable = Methods[type];
            if (type.Parent == null)
                return currentVirtualTable;

            var parentVirtualTable = GetVirtualTable(type.Parent);
            foreach (var (method, i) in parentVirtualTable.Select((x, i) => (x, i)))
            {
                int index;
                if ((index = currentVirtualTable.IndexOf(method)) < 0) continue;
                parentVirtualTable[i] = currentVirtualTable[index];
                currentVirtualTable.RemoveAt(index);
            }

            parentVirtualTable.ToList().AddRange(currentVirtualTable);

            return parentVirtualTable;
        }
    }

    public interface IMethodEnvironment
    {
        void AddMethod(CoolType type, string method, List<CoolType> formals, CoolType returnType);
        bool GetMethod(CoolType type, string method, out CoolMethod CoolMethod);
        bool GetMethodOnIt(CoolType type, string method, out CoolMethod CoolMethod);
        IList<CoolMethod> GetVirtualTable(CoolType type);
    }
}
