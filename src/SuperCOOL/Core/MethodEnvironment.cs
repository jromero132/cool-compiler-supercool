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
        public void AddMethod(CoolType type, string method, List<CoolType> formals, CoolType returnType, ISymbolTable symbolTable)
        {
            if (!Methods.ContainsKey(type))
                Methods[type] = new List<CoolMethod>();
            Methods[type].Add(new CoolMethod(type,method, formals, returnType,symbolTable));
        }
        public bool GetMethodIfDef(CoolType type, string method, out CoolMethod CoolMethod)
        {
            CoolMethod = new NullMethod(method);
            if (type is NullType)
                return true;
            if (type is SelfType selftype)
                return GetMethodIfDef(selftype.ContextType,method,out CoolMethod);
            if (GetMethodOnIt(type, method, out CoolMethod))
                return true;
            if (type.Parent != null)

                return GetMethodIfDef(type.Parent, method, out CoolMethod);
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
            var currentVirtualTable = Methods[type].ToList();
            if (type.Parent == null)
                return currentVirtualTable;

            var parentVirtualTable = GetVirtualTable(type.Parent).ToList();
            for (int i = 0; i < parentVirtualTable.Count; i++)
            {
                int index;
                for (index = 0; index < currentVirtualTable.Count; index++)
                    if (currentVirtualTable[index].Name == parentVirtualTable[i].Name)
                        break;
                if (index == currentVirtualTable.Count)
                    continue;
                parentVirtualTable[i] = currentVirtualTable[index];
                currentVirtualTable.RemoveAt(index);
            }

            return parentVirtualTable.Concat(currentVirtualTable).ToList();
        }

        public CoolMethod GetMethod(CoolType type, string method)
        {
            if (GetMethodOnIt(type, method, out var CoolMethod))
                return CoolMethod;
            if (type.Parent == null)
                return null;
            GetMethodIfDef(type.Parent, method, out CoolMethod);
            return CoolMethod;
        }

    }

    public interface IMethodEnvironment
    {
        void AddMethod(CoolType type, string method, List<CoolType> formals, CoolType returnType,ISymbolTable symbolTable);
        bool GetMethodIfDef(CoolType type, string method, out CoolMethod CoolMethod);
        CoolMethod GetMethod(CoolType type, string method);
        bool GetMethodOnIt(CoolType type, string method, out CoolMethod CoolMethod);
        IList<CoolMethod> GetVirtualTable(CoolType type);
    }
}
